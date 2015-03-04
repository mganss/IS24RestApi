%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe ..\IS24RestApi.sln /t:Clean,Rebuild /p:Configuration=Release /fileLogger
set spec=".\IS24RestApi.generated.nuspec"
set projs=..\IS24RestApi\IS24RestApi.csproj ..\IS24RestApi.Net40\IS24RestApi.Net40.csproj
if exist %spec% del %spec%
..\IS24RestApi.Pack\bin\Release\IS24RestApi.Pack.exe .\IS24RestApi.nuspec %spec% %projs%
.\NuGet.exe update -self
.\NuGet.exe pack -sym %spec%
if exist %spec% del %spec%
