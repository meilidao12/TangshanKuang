@echo off
@title Ð¶ÔØWindows·þÎñ
path %SystemRoot%\Microsoft.NET\Framework\v4.0.30319
echo==============================================================
echo= 
echo         VER:0.01
echo=
echo==============================================================
@echo off 
InstallUtil.exe  %~dp0\ZTGPRSService.exe

sc config GPRSService start= auto

del *.InstallLog
del *.InstallState
pause