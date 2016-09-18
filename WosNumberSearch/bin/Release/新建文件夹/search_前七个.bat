@echo off
start "" WosNumberSearch.exe ut更新tc.txt 0 100000
choice /t 10 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 100000 100000
choice /t 10 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 200000 100000
choice /t 10 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 300000 100000
choice /t 5 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 400000 100000
choice /t 5 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 500000 100000
choice /t 5 /d y /n >nul
start "" WosNumberSearch.exe ut更新tc.txt 600000 100000
end