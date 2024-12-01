dotnet tool uninstall minimalhmgenerator
dotnet nuget locals all --clear
cd DotnetMinimalHostingModelGenerator/nupkg
rm -rf *
cd ../
dotnet pack --configuration Release
cd ..
dotnet tool install --add-source ./DotnetMinimalHostingModelGenerator/nupkg MinimalHMGenerator