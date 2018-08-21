@echo off
cls

dotnet restore
if errorlevel 1 (
  exit /b %errorlevel%
)

dotnet test Tests
if errorlevel 1 (
  exit /b %errorlevel%
)

dotnet test CoreFsTests
if errorlevel 1 (
  exit /b %errorlevel%
)
