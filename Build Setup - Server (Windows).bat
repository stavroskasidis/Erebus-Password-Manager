@echo off
set root=%~dp0
rmdir "SetupOutput\Server-Windows" /S /Q
dotnet restore "%root%Erebus\Server\Erebus.Server"
xcopy "Erebus\InstallationTemplates\Server-Windows" "SetupOutput\Server-Windows\" /E /Y
dotnet publish "%root%Erebus\Server\Erebus.Server" --output "%root%SetupOutput\Server-Windows\site_files" --configuration Release
pause