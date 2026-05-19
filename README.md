# 🏫 School Portal — Microservices Architecture

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Docker Compose](https://img.shields.io/badge/Docker%20Compose-✔-2496ED?logo=docker)](https://docs.docker.com/compose/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

A modern school management system built on a **microservices architecture** with two fully isolated ASP.NET Core MVC services, each backed by its own SQL Server database — ensuring complete **data autonomy and fault isolation**.

---

## Architecture Diagram

```
┌──────────────────────────────────────────────────────────────────────────┐
│                          Docker Network                                  │
│                       (school-network)                                   │
│                                                                          │
│  ┌───────────────────────────┐          ┌───────────────────────────┐   │
│  │     students-mvc          │          │      grades-mvc           │   │
│  │  (ASP.NET Core 8.0 MVC)   │          │  (ASP.NET Core 8.0 MVC)   │   │
│  │                           │          │                           │   │
│  │  ┌─────────────────────┐  │          │  ┌─────────────────────┐  │   │
│  │  │   StudentsDB        │  │          │  │   GradesDB          │  │   │
│  │  │   (SQL Server)      │  │          │  │   (SQL Server)      │  │   │
│  │  └─────────────────────┘  │          │  └─────────────────────┘  │   │
│  │                           │          │                           │   │
│  │  Port: 8080 (container)  │          │  Port: 8080 (container)  │   │
│  │  Host: 5001              │          │  Host: 5002              │   │
│  └──────────┬────────────────┘          └──────────┬────────────────┘   │
│             │                                     │                     │
│             │        ┌──────────────────┐          │                     │
│             └────────►  SQL Server      ◄──────────┘                     │
│                      │  (Shared Host)   │                               │
│                      │  school-portal-db│                               │
│                      │  Named Volume:   │                               │
│                      │  school-portal-  │                               │
│                      │  data            │                               │
│                      └──────────────────┘                               │
│                                                                          │
│  ┌─ grades-mvc ────────────────────────────────────────────────┐        │
│  │                                                              │        │
│  │  GradesController ──── HttpClient ───────► students-mvc:8080 │        │
│  │                         │                  /api/students     │        │
│  │                         │                  /api/students/{id}│        │
│  │                         ▼                                    │        │
│  │                  StudentsServiceClient                        │        │
│  │                  (resilient wrapper with                      │        │
│  │                   try/catch + fallback UI)                    │        │
│  └──────────────────────────────────────────────────────────────┘        │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## Technical Stack

| Layer          | Technology                                      |
|----------------|-------------------------------------------------|
| **Backend**    | ASP.NET Core 8.0 (MVC) — C#                     |
| **Frontend**   | Razor Views + Bootstrap 5.3 + Bootstrap Icons   |
| **Styling**    | Custom CSS (Indigo theme, animations, gradients) |
| **Database**   | SQL Server 2022 (per-service isolated databases) |
| **ORM**        | Entity Framework Core 8.0 + Code-First Migrations|
| **Container**  | Docker + Docker Compose                          |
| **Network**    | Custom Docker bridge network (`school-network`)  |
| **Font**       | Inter (Google Fonts)                             |

---

## How to Run

```bash
git clone https://github.com/Mohamed-ehab-mohy/-School-Portal.git
cd School-Portal
docker compose up --build
```

**That's it.** No manual SQL Server setup. No manual migration runs. No `.env` tweaking required.

Auto-migrations and data seeding (20 students + 20 grades) execute automatically on first startup. The system is fully self-contained.

| Service         | URL                        |
|-----------------|----------------------------|
| Students Portal | http://localhost:5001      |
| Grades Portal   | http://localhost:5002      |

---

## Advanced Features

### 🔁 Resiliency & Fallback UI
If the students service goes down, the grades service **does not crash**. The `StudentsServiceClient` wraps all HTTP calls in `try/catch` blocks for both `HttpRequestException` and `TaskCanceledException`. When the service is unavailable, grades pages gracefully display **student IDs** instead of names, with a polished orange warning banner (`alert-warning` + fade-in animation).

### 🌍 Multi-Environment Setup
The project seamlessly runs in two environments:
- **Local development** — services communicate via `localhost:5001`
- **Inside Docker** — services communicate via container name `students-mvc:8080`

No manual connection string changes needed between environments.

### ⚡ Layer Caching Optimization
Dockerfiles are structured in **two stages**:
1. **Restore layer** — NuGet packages are restored and cached as a separate layer
2. **Publish layer** — only the source code is rebuilt on subsequent runs

This means repeated `docker compose up --build` calls are significantly faster since the package restore layer is reused from cache unless `*.csproj` changes.

### 🗄️ Data Seeding via EF Core HasData()
20 students and 20 grades are seeded directly in `OnModelCreating()` using `HasData()`, generating proper `InsertData`/`DeleteData` migration operations. No runtime seed scripts — the data is part of the migration history.

### 🔒 Data Isolation (per-service database)
Each microservice has its **own SQL Server database** (`StudentsDB` / `GradesDB`) on the same database host. This guarantees:
- No accidental cross-service data corruption
- Independent schema evolution
- True microservice autonomy

### 🎨 Modern UI/UX
- Indigo gradient hero sections with decorative shapes
- Floating label form inputs with `is-valid` green border feedback
- Responsive card grids, hover effects, and micro-animations
- 3-column footer with brand-specific social link hover effects (GitHub white glow, LinkedIn blue glow)
- Delete confirmation with animated pulse button
- Security & Architecture privacy page with live security metrics badges

---

## Project Structure

```
School-Portal/
├── students-mvc/           # Students microservice
│   ├── Controllers/
│   ├── Data/               # ApplicationDbContext + SeedStudents migration
│   ├── Models/             # Student entity
│   ├── Views/
│   └── wwwroot/
├── grades-mvc/             # Grades microservice
│   ├── Controllers/        # GradesController + HomeController
│   ├── Data/               # GradesDbContext + SeedGrades migration
│   ├── Models/             # Grade entity
│   ├── Services/           # StudentsServiceClient (HTTP)
│   ├── ViewModels/         # GradeFormViewModel + GradeViewModel
│   ├── Views/
│   └── wwwroot/
├── docker-compose.yml      # Full orchestration
├── .env                    # SQL Server password (gitignored)
├── .env.example            # Template for .env
└── README.md
```

---

## Author

**Mohamed Ehab** — Full-stack .NET Developer

[![GitHub](https://img.shields.io/badge/GitHub-Mohamed--ehab--mohy-181717?logo=github)](https://github.com/Mohamed-ehab-mohy)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Mohamed%20Ehab-0A66C2?logo=linkedin)](https://linkedin.com/in/mohamed-ehab-mohy)

