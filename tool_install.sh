dotnet tool uninstall  minimalhmgenerator --global
dotnet nuget locals all --clear
cd DotnetMinimalHostingModelGenerator/nupkg
rm -rf *
cd ../
dotnet pack --configuration Release
cd ..
dotnet tool install --global --add-source ./DotnetMinimalHostingModelGenerator/nupkg MinimalHMGenerator
dotnet restore