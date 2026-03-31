@echo off
chcp 65001 >nul
echo   *** СБОРКА ПЛАТЁЖНОЙ СИСТЕМЫ ***
echo.

where g++ >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo [ОШИБКА] g++ не найден! Установите MinGW.
    pause
    exit /b 1
)

echo [1/2] Компиляция: 2labwithpat.cpp
g++ -o payment_backend_withpat.exe 2labwithpat.cpp -std=c++11 -O2
if %ERRORLEVEL% neq 0 (
    echo [ОШИБКА] Ошибка компиляции!
    pause
    exit /b 1
)

echo [2/2] Компиляция: 2labwithoutpat.cpp
g++ -o payment_backend_withoutpat.exe 2labwithoutpat.cpp -std=c++11 -O2
if %ERRORLEVEL% neq 0 (
    echo [ОШИБКА] Ошибка компиляции!
    pause
    exit /b 1
)

echo.
echo   *** СБОРКА ЗАВЕРШЕНА! ***
echo.
echo Запуск: python payment_gui.py
pause