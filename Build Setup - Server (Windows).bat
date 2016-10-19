@echo off
set root=%~dp0
rmdir "SetupOutput\Server-Windows" /S /Q
dotnet restore "Erebus\.vs\restore.dg"
xcopy "Erebus\InstallationTemplates\Server-Windows" "SetupOutput\Server-Windows\" /E /Y
dotnet publish "%root%Erebus\Server\Erebus.Server" --framework netcoreapp1.0 --output "%root%SetupOutput\Server-Windows\site_files" --configuration Release
pause