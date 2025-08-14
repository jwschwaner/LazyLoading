# LazyLoading

A full-stack application with a .NET Core backend API, React+TypeScript frontend, and PostgreSQL database.

## Prerequisites

Before you begin, ensure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) - Required to run the containerized application stack
- PowerShell (Windows) or Bash (Linux/macOS) - For running the helper scripts

## Project Structure

- `LazyLoading/` - .NET Core backend solution
  - `LazyLoading.Api` - REST API project
  - `LazyLoading.Application` - Business logic and application services
  - `LazyLoading.Domain` - Domain entities and business rules
  - `LazyLoading.Infrastructure` - Database, external services integration
- `client/` - React+TypeScript frontend application built with Vite
- `docker-compose.yml` - Docker Compose configuration for the entire stack
- `run.ps1` - PowerShell script for managing the application (Windows)
- `run.sh` - Bash script for managing the application (Linux/macOS)

## Getting Started

### Running the Application

1. Clone this repository:
   ```
   git clone https://github.com/jwschwaner/LazyLoading.git
   cd LazyLoading
   ```

2. Start the application stack:

   **Windows (PowerShell):**
   ```powershell
   ./run.ps1 start
   ```

   **Linux/macOS (Bash):**
   ```bash
   chmod +x ./run.sh  # Make the script executable (first time only)
   ./run.sh start
   ```

3. Access the application:
   - Frontend: [http://localhost:3000](http://localhost:3000)
   - API: [http://localhost:8080](http://localhost:8080)
   - PostgreSQL: localhost:5432 (User: postgres, Password: postgres, DB: LazyLoading)

### Available Commands

Both scripts (`run.ps1` for Windows and `run.sh` for Linux/macOS) support the following commands:

- `start` - Start the application stack (default)
- `stop` - Stop the application stack
- `restart` - Restart the application stack
- `rebuild` - Rebuild and start the application stack (useful after code changes)
- `logs` - View logs from all containers
- `migrate` - Run EF Core database migrations
- `clean` - Remove all containers, volumes, and networks (will delete all data)
- `help` - Show the help message

Example usage:
```
./run.ps1 rebuild  # Windows
./run.sh rebuild   # Linux/macOS
```

## Development Workflow

1. Make changes to the code in either the backend (.NET) or frontend (React)
2. Rebuild the application stack:
   ```
   ./run.ps1 rebuild  # Windows
   ./run.sh rebuild   # Linux/macOS
   ```
3. View logs if needed:
   ```
   ./run.ps1 logs  # Windows
   ./run.sh logs   # Linux/macOS
   ```

## Database Migrations

After making changes to entity models, you can run migrations:

```
./run.ps1 migrate  # Windows
./run.sh migrate   # Linux/macOS
```

## Cleaning Up

To completely remove all containers, volumes, and networks (this will delete all data):

```
./run.ps1 clean  # Windows
./run.sh clean   # Linux/macOS
```