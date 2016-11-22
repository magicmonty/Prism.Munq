@echo off

paket.exe restore
IF ERRORLEVEL 1 exit /b 1


.\packages\build\FAKE\tools\FAKE.exe %*