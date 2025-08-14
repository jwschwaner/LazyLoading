#!/bin/bash
# run.sh - Script to manage the LazyLoading application stack

set -e

# Project information
PROJECT_NAME="LazyLoading"

# Function to display help information
show_help() {
    echo "LazyLoading Stack Management Script"
    echo "Usage: ./run.sh [action]"
    echo ""
    echo "Actions:"
    echo "  start    - Start the application stack (default)"
    echo "  stop     - Stop the application stack"
    echo "  restart  - Restart the application stack"
    echo "  rebuild  - Rebuild and start the application stack"
    echo "  logs     - View logs from all containers"
    echo "  migrate  - Run EF Core migrations"
    echo "  clean    - Remove all containers, volumes, and networks"
    echo "  help     - Show this help message"
}

# Function to start the application stack
start_stack() {
    echo -e "\e[32mStarting $PROJECT_NAME stack...\e[0m"
    docker-compose up -d
    echo -e "\e[32mStack is now running! Access the API at http://localhost:8080\e[0m"
    echo -e "\e[32mPostgreSQL is available at localhost:5432\e[0m"
}

# Function to stop the application stack
stop_stack() {
    echo -e "\e[33mStopping $PROJECT_NAME stack...\e[0m"
    docker-compose down
    echo -e "\e[32mStack has been stopped\e[0m"
}

# Function to restart the application stack
restart_stack() {
    stop_stack
    start_stack
}

# Function to rebuild and start the application stack
rebuild_stack() {
    echo -e "\e[33mRebuilding $PROJECT_NAME stack...\e[0m"
    docker-compose up -d --build
    echo -e "\e[32mStack has been rebuilt and is now running!\e[0m"
}

# Function to show container logs
show_logs() {
    echo -e "\e[36mShowing logs for $PROJECT_NAME stack...\e[0m"
    docker-compose logs -f
}

# Function to run database migrations
run_migrations() {
    echo -e "\e[36mRunning database migrations...\e[0m"
    
    # Check if the API container is running
    if [ -z "$(docker-compose ps -q api)" ]; then
        echo -e "\e[33mStarting API container to run migrations...\e[0m"
        docker-compose up -d api
    fi
    
    # Execute migrations inside the container
    docker-compose exec api dotnet ef database update --project /app/LazyLoading.Infrastructure --startup-project /app/LazyLoading.Api
    
    echo -e "\e[32mMigrations completed successfully\e[0m"
}

# Function to clean up all resources
clean_stack() {
    echo -e "\e[31mRemoving all containers, volumes, and networks for $PROJECT_NAME...\e[0m"
    
    read -p "Are you sure you want to remove all containers, volumes, and networks? This will delete all data. (y/n) " confirmation
    
    if [[ $confirmation =~ ^[Yy]$ ]]; then
        docker-compose down -v
        echo -e "\e[32mAll containers, volumes, and networks have been removed\e[0m"
    else
        echo -e "\e[33mClean operation canceled\e[0m"
    fi
}

# Parse command line arguments
ACTION=${1:-start}

# Execute the requested action
case "$ACTION" in
    start)
        start_stack
        ;;
    stop)
        stop_stack
        ;;
    restart)
        restart_stack
        ;;
    rebuild)
        rebuild_stack
        ;;
    logs)
        show_logs
        ;;
    migrate)
        run_migrations
        ;;
    clean)
        clean_stack
        ;;
    help)
        show_help
        ;;
    *)
        echo -e "\e[31mUnknown action: $ACTION\e[0m"
        show_help
        exit 1
        ;;
esac
