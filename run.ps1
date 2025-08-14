#!/usr/bin/env pwsh
# run.ps1 - Script to manage the LazyLoading application stack

param (
    [string]$action = "help"
)

$ErrorActionPreference = "Stop"

# Project information
$projectName = "LazyLoading"

function Show-Help {
    Write-Host "LazyLoading Stack Management Script"
    Write-Host "Usage: ./run.ps1 [action]"
    Write-Host ""
    Write-Host "Actions:"
    Write-Host "  start    - Start the application stack"
    Write-Host "  stop     - Stop the application stack"
    Write-Host "  restart  - Restart the application stack"
    Write-Host "  rebuild  - Rebuild and start the application stack"
    Write-Host "  logs     - View logs from all containers"
    Write-Host "  migrate  - Run EF Core migrations"
    Write-Host "  clean    - Remove all containers, volumes, and networks"
    Write-Host "  help     - Show this help message"
}

function Start-Stack {
    Write-Host "Starting $projectName stack..." -ForegroundColor Green
    docker-compose up -d
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to start $projectName stack" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "Stack is now running! Access the API at http://localhost:8080" -ForegroundColor Green
    Write-Host "Frontend UI is available at http://localhost:3000" -ForegroundColor Green
    Write-Host "PostgreSQL is available at localhost:5432" -ForegroundColor Green
}

function Stop-Stack {
    Write-Host "Stopping $projectName stack..." -ForegroundColor Yellow
    docker-compose down
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to stop $projectName stack" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "Stack has been stopped" -ForegroundColor Green
}

function Restart-Stack {
    Stop-Stack
    Start-Stack
}

function Rebuild-Stack {
    Write-Host "Rebuilding $projectName stack..." -ForegroundColor Yellow
    docker-compose up -d --build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to rebuild $projectName stack" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "Stack has been rebuilt and is now running!" -ForegroundColor Green
    Write-Host "Access the API at http://localhost:8080" -ForegroundColor Green
    Write-Host "Frontend UI is available at http://localhost:3000" -ForegroundColor Green
    Write-Host "PostgreSQL is available at localhost:5432" -ForegroundColor Green
}

function Show-Logs {
    Write-Host "Showing logs for $projectName stack..." -ForegroundColor Cyan
    docker-compose logs -f
}

function Run-Migrations {
    Write-Host "Running database migrations..." -ForegroundColor Cyan
    
    # Check if the API container is running
    $apiRunning = docker-compose ps -q api
    if (-not $apiRunning) {
        Write-Host "Starting API container to run migrations..." -ForegroundColor Yellow
        docker-compose up -d api
    }
    
    # Execute migrations inside the container
    docker-compose exec api dotnet ef database update --project /app/LazyLoading.Infrastructure --startup-project /app/LazyLoading.Api
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to run migrations" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    
    Write-Host "Migrations completed successfully" -ForegroundColor Green
}

function Clean-Stack {
    Write-Host "Removing all containers, volumes, and networks for $projectName..." -ForegroundColor Red
    
    $confirmation = Read-Host "Are you sure you want to remove all containers, volumes, and networks? This will delete all data. (y/n)"
    
    if ($confirmation -eq "y" -or $confirmation -eq "Y") {
        docker-compose down -v
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Failed to clean up $projectName stack" -ForegroundColor Red
            exit $LASTEXITCODE
        }
        Write-Host "All containers, volumes, and networks have been removed" -ForegroundColor Green
    }
    else {
        Write-Host "Clean operation canceled" -ForegroundColor Yellow
    }
}

# Execute the requested action
switch ($action.ToLower()) {
    "start" { Start-Stack }
    "stop" { Stop-Stack }
    "restart" { Restart-Stack }
    "rebuild" { Rebuild-Stack }
    "logs" { Show-Logs }
    "migrate" { Run-Migrations }
    "clean" { Clean-Stack }
    "help" { Show-Help }
    default {
        Write-Host "Unknown action: $action" -ForegroundColor Red
        Show-Help
        exit 1
    }
}
