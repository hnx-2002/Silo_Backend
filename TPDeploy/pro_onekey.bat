@REM 设置变量
@set thisProject=PTools_PSilo
@set targetUrl=<目标地址>
@set deployUser=%~1_pro
@set buildType=pro
@set publishType=Release

chcp 65001

@REM 记录程序开始时间
@set start=%time%
@echo Program started at %start%

@REM 打包前端
cd ../Frontend
call pnpm i
call pnpm run buildpro

@REM 清空发布目录所有文件
cd ../Backend_cs
del /s /q bin\publish_%buildType%\*.* > nul
rmdir /s /q bin\publish_%buildType% > nul

@REM 如果没有绑定TCDRI.NuGetServer需要执行下边代码
@REM dotnet nuget add source 'http://10.1.26.43:443/' --name "TCDRI.NuGetServer"

@REM 打包后端
dotnet publish %thisProject%.csproj -o bin/publish_%buildType% -c %publishType% -f net8.0 -r linux-x64 --self-contained

cd ../TPDeploy

@REM 混淆操作
TPConfuser.exe ../Backend_cs/bin/publish_%buildType%/ConfuseConfig.crproj

cd ../Backend_cs
xcopy bin\publish_%buildType%\Confused bin\publish_%buildType% /Y
del /s /q bin\publish_%buildType%\*.pdb > nul
@REM del /s /q bin\publish_%buildType%\*.xml > nul

@REM 测试环境删掉！！！
IF NOT "%buildType%"=="pro" (
    del /s /q bin\publish_%buildType%\appsettings.Production.json > nul
) ELSE (
    del /s /q bin\publish_%buildType%\appsettings.Development.json > nul
    del /s /q bin\publish_%buildType%\appsettings.Staging.json > nul
)
rd /s /q bin\publish_%buildType%\Confused
del /s /q bin\publish_%buildType%\ConfuseConfig.crproj > nul

cd ../TPDeploy

@echo off
setlocal enabledelayedexpansion
set "this_path=%cd%"
set "new_path=%this_path:TPDeploy=%Backend_cs\bin\publish_%buildType%\%thisProject%.dll"
set "mpath=%new_path:\=\\%"
echo on
@REM echo %mpath%
TPCDClient.exe -c %buildType% -u http://%targetUrl%:9416 -p %mpath% -a %deployUser%

@REM 记录程序结束时间
@set end=%time%

@echo time1 %start%
@echo time2 %end%

@REM 计算耗时
