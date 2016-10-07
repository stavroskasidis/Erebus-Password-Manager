@echo off

:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
    IF "%PROCESSOR_ARCHITECTURE%" EQU "amd64" (
>nul 2>&1 "%SYSTEMROOT%\SysWOW64\cacls.exe" "%SYSTEMROOT%\SysWOW64\config\system"
) ELSE (
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
)

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params = %*:"=""
    echo UAC.ShellExecute "cmd.exe", "/c ""%~s0"" %params%", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    CD /D "%~dp0"
:--------------------------------------
  
@echo off
cd /d "%~dp0"
echo ==== Erebus Server - Windows Installation ====
echo.
echo Configuring IIS. This may take a while ...
DISM /Online /NoRestart /Enable-Feature /all /FeatureName:IIS-ASP /FeatureName:IIS-ASPNET /FeatureName:IIS-BasicAuthentication /FeatureName:IIS-CGI /FeatureName:IIS-CommonHttpFeatures /FeatureName:IIS-CustomLogging /FeatureName:IIS-DefaultDocument /FeatureName:IIS-DirectoryBrowsing /FeatureName:IIS-HealthAndDiagnostics /FeatureName:IIS-HostableWebCore /FeatureName:IIS-HttpCompressionDynamic /FeatureName:IIS-HttpCompressionStatic /FeatureName:IIS-HttpErrors /FeatureName:IIS-HttpLogging /FeatureName:IIS-HttpRedirect /FeatureName:IIS-HttpTracing /FeatureName:IIS-IIS6ManagementCompatibility /FeatureName:IIS-IPSecurity /FeatureName:IIS-ISAPIExtensions /FeatureName:IIS-ISAPIFilter /FeatureName:IIS-LegacyScripts /FeatureName:IIS-LegacySnapIn /FeatureName:IIS-LoggingLibraries /FeatureName:IIS-ManagementConsole /FeatureName:IIS-ManagementScriptingTools /FeatureName:IIS-ManagementService /FeatureName:IIS-Metabase /FeatureName:IIS-NetFxExtensibility /FeatureName:IIS-Performance /FeatureName:IIS-RequestFiltering /FeatureName:IIS-RequestMonitor /FeatureName:IIS-Security /FeatureName:IIS-ServerSideIncludes /FeatureName:IIS-StaticContent /FeatureName:IIS-URLAuthorization /FeatureName:IIS-WebDAV /FeatureName:IIS-WebServer /FeatureName:IIS-WebServerManagementTools /FeatureName:IIS-WebServerRole /FeatureName:IIS-WMICompatibility /FeatureName:WAS-ConfigurationAPI /FeatureName:WAS-NetFxEnvironment /FeatureName:WAS-ProcessModel /FeatureName:WAS-WindowsActivationService
echo Installing .Net Core Windows Hosting ...
start /wait DotNetCore.1.0.1-WindowsHosting.exe /s
net stop was /y > nul
net start w3svc > nul
echo Copying files ...
xcopy site_files %SystemDrive%\inetpub\Erebus\ /E /Y > nul
echo Creating site on IIS ...
%windir%\system32\inetsrv\appcmd add site /name:Erebus /physicalPath:%SystemDrive%\inetpub\Erebus /bindings:http/*:8080: > nul
%windir%\system32\inetsrv\appcmd add apppool /name:Erebus /managedRuntimeVersion: > nul
%windir%\system32\inetsrv\appcmd set app "Erebus/" /applicationPool:"Erebus" > nul
netsh advfirewall firewall show rule name="Erebus" > nul
if not ERRORLEVEL 1 ( netsh advfirewall firewall delete rule name="Erebus" > nul )
netsh advfirewall firewall add rule name="Erebus" dir=in action=allow protocol=TCP localport=8080 > nul
echo Setting site folder permissions ...
icacls %SystemDrive%\inetpub\Erebus /grant IIS_IUSRS:(OI)(CI)(F) /T > nul
echo.
echo Application installed and running at:
echo http://%COMPUTERNAME%:8080/
echo.
pause
start http://%COMPUTERNAME%:8080