using System.CommandLine;
using DotnetMinimalHostingModelGenerator.FileTransformation;

var startupFileOption = new Option<FileInfo?>(
    name: "--startup-file",
    description: "The Startup.cs file to transform.");

var outputFilePathOption = new Option<string?>(
    name: "--output-file-path",
    description: "The output file path to write to.");

startupFileOption.AddValidator(result =>
{
    var file = result.GetValueOrDefault<FileInfo>();
    if (file is { Exists: false })
    {
        result.ErrorMessage = $"Startup file '{file.FullName}' does not exist.";
    }
});

outputFilePathOption.AddValidator(result =>
{
    var path = result.GetValueOrDefault<string>();
    if (!Directory.Exists(path))
    {
        result.ErrorMessage = $"Output file path '{path}' does not exist.";
    }
});

var rootCommand = new RootCommand("Tool to generate DotnetMinimal hosting model.");
var transformCommand = new Command("transform", "Transform the startup.cs file")
{
    startupFileOption,
    outputFilePathOption,
};
transformCommand.SetHandler((context) =>
{
    var startupFile = context.ParseResult.GetValueForOption(startupFileOption);
    var outputFilePath = context.ParseResult.GetValueForOption(outputFilePathOption);
    var isSuccess = FileTransformer.TransformFile(startupFile!, outputFilePath!);
    
    if (isSuccess)
    {
        Console.WriteLine($"Successfully transformed {startupFile!.FullName}");
    }

    context.ExitCode = isSuccess ? 0 : 1;
});

rootCommand.AddCommand(transformCommand);
return await rootCommand.InvokeAsync(args);