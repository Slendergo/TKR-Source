@echo off
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :80') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im server.exe