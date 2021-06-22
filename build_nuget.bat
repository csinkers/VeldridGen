@echo off
if not exist nuget mkdir nuget
dotnet clean -c Debug   VeldridGen.NuGet.sln
dotnet clean -c Release VeldridGen.NuGet.sln
rd /S /Q .\bin
dotnet build -c Release VeldridGen\VeldridGen.csproj
dotnet build -c Release VeldridGen.Interfaces\VeldridGen.Interfaces.csproj
dotnet pack  -c Release VeldridGen\VeldridGen.csproj
dotnet pack  -c Release VeldridGen.Interfaces\VeldridGen.Interfaces.csproj
move /Y VeldridGen\bin\Release\*.nupkg nuget
move /Y VeldridGen.Interfaces\bin\Release\*.nupkg nuget

:: dotnet nuget add source https://nuget.pkg.github.com/csinkers/index.json -n github -u username -p token
:: dotnet nuget push "src/bin/Release/VeldridGen.VERSION.nupkg" --source "github"
:: dotnet nuget push "src/bin/Release/VeldridGen.VERSION.nupkg" --source nuget.org -k NUGET_KEY
:: dotnet nuget remove source github

