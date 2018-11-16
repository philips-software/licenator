@echo OFF
echo Creating release for Licenator...

dotnet pack Licenator -c Release -o publish /p:VersionPrefix=v0.0.1

REM dotnet publish -c Release -o publish /p:VersionPrefix=${VERSION_PREFIX} /p:VersionSuffix=${GIT_COMMIT_ID}

echo Installing to local tool cache...

dotnet tool uninstall --global licenator
dotnet tool install --global --add-source .\Licenator\publish\ licenator

echo Licenator installed to this machine's global tool cache. You can use this to test the tool.
echo You can publish .\Licenator\publish\ to the NuGet Gallery.
