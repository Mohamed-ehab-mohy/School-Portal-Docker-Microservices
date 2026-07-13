# School Portal - Microservices Architecture

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Docker Compose](https://img.shields.io/badge/Docker%20Compose-✔-2496ED?logo=docker)](https://docs.docker.com/compose/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A modern school management system built on a **microservices architecture** with two fully isolated ASP.NET Core MVC services, each backed by its own PostgreSQL database ensuring complete **data autonomy and fault isolation**.

**Deploy for free** on Render, Fly.io, or Koyeb in minutes.

---

## Live Demo

| Service | URL |
|---------|-----|
| Students Portal | [localhost:5001](http://localhost:5001) |
| Grades Portal | [localhost:5002](http://localhost:5002) |
| Health Check | [localhost:5001/health](http://localhost:5001/health) |

---

## Quick Start

```bash
# Clone the repository
git clone https://github.com/Mohamed-ehab-mohy/School-Portal-Docker-Microservices.git
cd School-Portal-Docker-Microservices

# Copy environment file
cp .env.example .env

# Start all services
docker compose up --build
```

**That's it.** No manual database setup. No manual migration runs. Auto-migrations and data seeding (20 students + 20 grades) execute automatically on first startup.

---

## Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                        Docker Network                               │
│                     (school-network)                                 │
│                                                                      │
│  ┌─────────────────────────┐       ┌─────────────────────────┐     │
│  │      students-mvc       │       │       grades-mvc        │     │
│  │  (ASP.NET Core 10 MVC)  │       │  (ASP.NET Core 10 MVC)  │     │
│  │                         │       │                         │     │
│  │  ┌───────────────────┐  │       │  ┌───────────────────┐  │     │
│  │  │   StudentsDB      │  │       │  │   GradesDB        │  │     │
│  │  │   (PostgreSQL)    │  │       │  │   (PostgreSQL)    │  │     │
│  │  └───────────────────┘  │       │  └───────────────────┘  │     │
│  │                         │       │                         │     │
│  │  Port: 8080 (container)│       │  Port: 8080 (container)│     │
│  │  Host: 5001            │       │  Host: 5002            │     │
│  │                         │       │                         │     │
│  │  GET /api/students      │◄──────│  StudentsServiceClient  │     │
│  │  GET /api/students/{id} │       │  (HTTP + Resilience)    │     │
│  └─────────────────────────┘       └─────────────────────────┘     │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │                    PostgreSQL 16                             │   │
│  │  ├── students_db  (Identity + Students)                     │   │
│  │  └── grades_db    (Grades)                                  │   │
│  └──────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘
```

### Key Design Decisions

- **Database Isolation**: Each service owns its database. No shared schemas.
- **HTTP Communication**: Grades service calls Students service via REST API
- **Resilient Fallback**: If Students service is down, Grades shows student IDs instead of names
- **Health Checks**: Both services expose `/health` endpoints for monitoring
- **EF Core Migrations**: Proper migration-based schema management

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| **Backend** | ASP.NET Core 10 (MVC) — C# |
| **Frontend** | Razor Views + Bootstrap 5.3 + Bootstrap Icons |
| **Database** | PostgreSQL 16 (isolated per service) |
| **ORM** | Entity Framework Core 10 + Code-First Migrations |
| **Container** | Docker + Docker Compose |
| **Network** | Custom Docker bridge network (`school-network`) |
| **CI/CD** | GitHub Actions |

---

## Features

### Resiliency & Fallback UI
If the students service goes down, the grades service **does not crash**. The `StudentsServiceClient` wraps HTTP calls with try/catch blocks. When the service is unavailable, grades pages gracefully display **student IDs** instead of names.

### Multi-Environment Setup
- **Local development**: Services communicate via `localhost:5001`
- **Inside Docker**: Services communicate via container name `students-mvc:8080`
- **Production**: Environment variables configure all connection strings

### Layer Caching Optimization
Dockerfiles use **multi-stage builds**:
1. **Restore layer** — NuGet packages cached separately
2. **Publish layer** — Only source code rebuilt on changes

### Data Seeding via EF Core HasData()
20 students and 20 grades seeded directly in `OnModelCreating()` using `HasData()`. Data is part of the migration history — no runtime seed scripts.

### Data Isolation
Each microservice has its **own PostgreSQL database**:
- `students_db`: Identity tables + Students table
- `grades_db`: Grades table only

### Health Checks
Both services expose `/health` endpoints that verify database connectivity.

---

## API Endpoints

### Students Service (Port 5001)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/students` | Admin, Teacher | Get all students |
| GET | `/api/students/{id}` | — | Get student by ID |
| GET | `/health` | — | Health check |

### Grades Service (Port 5002)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/health` | — | Health check |

---

## Project Structure

```
School-Portal/
├── students-mvc/              # Students microservice
│   ├── Controllers/           # StudentsController, AccountController, HomeController
│   ├── Data/                  # ApplicationDbContext + Migrations/
│   ├── Models/                # Student, ApplicationUser, ViewModels
│   ├── Views/                 # Razor views
│   ├── Dockerfile
│   └── railway.json           # Railway deployment config
├── grades-mvc/                # Grades microservice
│   ├── Controllers/           # GradesController, HomeController
│   ├── Data/                  # GradesDbContext + Migrations/
│   ├── Models/                # Grade, Student (read-only mirror)
│   ├── Services/              # StudentsServiceClient (HTTP client)
│   ├── ViewModels/            # GradeFormViewModel, GradeViewModel
│   ├── Views/                 # Razor views
│   ├── Dockerfile
│   └── railway.json           # Railway deployment config
├── postgres/
│   └── init/                  # Database initialization scripts
├── .github/workflows/         # CI/CD pipeline
├── docker-compose.yml         # Full orchestration
├── render.yaml                # Render.com deployment
├── DEPLOYMENT.md              # Deployment guide
├── .env.example               # Environment template
└── README.md
```

---

## Free Deployment

Deploy this project for free on multiple platforms:

| Platform | Free Tier | Guide |
|----------|-----------|-------|
| **Render** | 750 hrs/month | [DEPLOYMENT.md](DEPLOYMENT.md#option-1-rendercom-recommended) |
| **Fly.io** | 3 VMs + 3GB storage | [DEPLOYMENT.md](DEPLOYMENT.md#option-2-flyio) |
| **Koyeb** | 1 free nano service | [DEPLOYMENT.md](DEPLOYMENT.md#option-3-koyeb) |

See [DEPLOYMENT.md](DEPLOYMENT.md) for detailed instructions.

---

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | `postgres` |
| `POSTGRES_PASSWORD` | PostgreSQL password | `YourStrongPassword123` |
| `ADMIN_PASSWORD` | Admin user password | `Admin@123` |
| `StudentsService__BaseUrl` | Students service URL | `http://students-mvc:8080` |

---

## Default Credentials

- **Admin Email**: `admin@school.com`
- **Admin Password**: `Admin@123` (change via `ADMIN_PASSWORD` env var)

---

## Development

### Prerequisites
- Docker Desktop
- .NET 10 SDK (for local development)

### Run Locally Without Docker
```bash
# Start PostgreSQL (via Docker or local install)
docker run -d --name postgres -e POSTGRES_PASSWORD=YourStrongPassword123 -p 5432:5432 postgres:16-alpine

# Run students-mvc
cd students-mvc
dotnet run

# Run grades-mvc (in another terminal)
cd grades-mvc
dotnet run
```

---

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## Author

**Mohamed Ehab** — Full-stack .NET Developer

[![GitHub](https://img.shields.io/badge/GitHub-Mohamed--ehab--mohy-181717?logo=github)](https://github.com/Mohamed-ehab-mohy)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Mohamed%20Ehab-0A66C2?logo=linkedin)](https://linkedin.com/in/mohamed-ehab-mohy)

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
