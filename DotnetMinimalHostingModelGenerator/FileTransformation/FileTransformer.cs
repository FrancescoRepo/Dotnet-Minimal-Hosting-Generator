using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetMinimalHostingModelGenerator.FileTransformation;

internal static class FileTransformer
{
    private const string ConfigureServicesMethodName = "ConfigureServices";
    private const string ConfigureMethodName = "Configure";
    private const string OutputFileName = "NewProgram.cs";

    /// <summary>
    /// Create a new version of the Program.cs with the new hosting model format starting from an input file that contains a Startup class
    /// </summary>
    /// <param name="inputFilePath">Input file to transform</param>
    /// <param name="outputFilePath">Output file path where to write the transformed file</param>
    /// <returns></returns>
    internal static bool TransformFile(FileInfo inputFilePath, string outputFilePath)
    {
        try
        {
            Console.WriteLine($"Processing file: {inputFilePath.FullName}");

            var csFilePath = inputFilePath.FullName;
            var csFileContent = File.ReadAllText(csFilePath);
            var tree = CSharpSyntaxTree.ParseText(csFileContent);
            var root = tree.GetCompilationUnitRoot();
            var usings = root.Usings;
            var nds = (NamespaceDeclarationSyntax)root.Members[0];

            var outputFile = usings.Select(usingDirective => usingDirective.ToString()).ToList();
            outputFile.Add("");
            outputFile.Add("var builder = WebApplication.CreateBuilder(args);");

            var startupClassDs =
                (ClassDeclarationSyntax?)nds.Members.FirstOrDefault(n => n is ClassDeclarationSyntax
                {
                    Identifier.ValueText: "Startup"
                });

            if (startupClassDs is null)
            {
                Console.Error.WriteLine("No Startup class found.");
                return false;
            }

            // Transform the startup class to the new hosting model
            foreach (var ds in startupClassDs.Members)
            {
                switch (ds)
                {
                    case MethodDeclarationSyntax mds:
                    {
                        var methodName = mds.Identifier.ValueText;
                        var methodBody = mds.Body?.ToString();
                        if (string.IsNullOrWhiteSpace(methodName)) continue;
                        if (methodBody == null) continue;

                        switch (methodName)
                        {
                            case ConfigureServicesMethodName:
                                methodBody = methodBody.Replace("services", "builder.Services")
                                    .Replace("Configuration", "builder.Configuration").TrimStart('{').TrimEnd('}');
                                outputFile.Add(methodBody);
                                break;
                            case ConfigureMethodName:
                                outputFile.Add("var app = builder.Build();");
                                outputFile.Add(methodBody.Replace("env.", "app.Environment.").TrimStart('{')
                                    .TrimEnd('}'));
                                outputFile.Add("app.Run();");
                                break;
                            //Copy all the remaining methods as they are
                            default:
                                outputFile.Add(mds.ToString());
                                break;
                        }

                        break;
                    }
                    case PropertyDeclarationSyntax { Identifier.ValueText: "Configuration" }:
                        continue;
                    default:
                    {
                        if (ds is not ConstructorDeclarationSyntax)
                        {
                            outputFile.Add(ds.ToString());
                        }

                        break;
                    }
                }
            }

            // Copy the rest of the file
            outputFile.AddRange(nds.Members.RemoveAt(nds.Members.IndexOf(startupClassDs)).Select(ds => ds.ToString()));

            //Write the output file
            File.WriteAllLines($"{outputFilePath}/{OutputFileName}", outputFile);
            return true;
        }
        catch (UnauthorizedAccessException e)
        {
            Console.Error.WriteLine($"The user has no permission to write in the output file path {outputFilePath}.");
            return false;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
            return false;
        }
    }
}