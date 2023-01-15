@echo off
setlocal enabledelayedexpansion
set i=0
for /f "tokens=*" %%a in ('dir /b /a-d *.png') do (
set /a i+=1
set "num=00!i!"
set "num=!num:~-3!"
ren "%%a" "!num!.png"
)