@echo off
setlocal
cd /d "%~dp0"
set "LOG=%~dp0build-csharp.log"
echo Build started %DATE% %TIME% > "%LOG%"
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto novswhere
for /f "usebackq tokens=*" %%i in (`"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"
if not defined MSBUILD goto nomsbuild
echo MSBuild: %MSBUILD% >> "%LOG%"
"%MSBUILD%" "Advanced SNES ROM Utility.sln" /t:Build /p:Configuration=Release /m /nologo /v:minimal /clp:Summary >> "%LOG%" 2>&1
echo EXITCODE=%ERRORLEVEL% >> "%LOG%"
goto end
:novswhere
echo vswhere not found >> "%LOG%"
goto end
:nomsbuild
echo MSBuild not found >> "%LOG%"
:end
echo Build finished %DATE% %TIME% >> "%LOG%"
