dotnet tool uninstall  minimalhmgenerator --global
dotnet nuget locals all --clear
cd DotnetMinimalHostingModelGenerator/nupkg
rm -rf *
cd ../
dotnet build --configuration Release
dotnet pack --configuration Release
dotnet tool install --global --add-source ./nupkg MinimalHMGenerator
dotnet restore