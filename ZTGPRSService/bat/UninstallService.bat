@echo off 
@title 卸载Windows服务
path %SystemRoot%\Microsoft.NET\Framework\v4.0.30319
echo==============================================================
echo=
echo          windows服务卸载
echo=
echo==============================================================
@echo off 
InstallUtil.exe /u  %~dp0\ZTGPRSService.exe
del *.InstallLog
pause