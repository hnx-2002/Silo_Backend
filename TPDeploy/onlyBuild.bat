@REM 设置变量
@set thisProject=PTools_PSilo
@set deployUser=%~1_onlyBuild
@set buildType=onlyBuild
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
del /s /q bin\publish_%buildType%\appsettings.Production.json > nul 
del /s /q bin\publish_%buildType%\appsettings.Development.json > nul
del /s /q bin\publish_%buildType%\appsettings.Staging.json > nul 
rd /s /q bin\publish_%buildType%\Confused
del /s /q bin\publish_%buildType%\ConfuseConfig.crproj > nul
rd /s /q bin\publish_%buildType%\AppSettings   
del /s /q bin\publish_%buildType%\Auth\Lic\TCDRILIC > nul

@REM 记录程序结束时间  
@set end=%time%  
 
@echo time1 %start%  
@echo time2 %end%  
  
@REM 计算耗时

