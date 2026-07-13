# School Portal - Microservices Architecture

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Docker Compose](https://img.shields.io/badge/Docker%20Compose-✔-2496ED?logo=docker)](https://docs.docker.com/compose/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3-FF6600?logo=rabbitmq)](https://www.rabbitmq.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A **portfolio-ready** school management system built on a **microservices architecture** with Docker containers, PostgreSQL, RabbitMQ messaging, API Gateway, role-based authentication, attendance tracking, notifications, observability stack, and CI/CD pipeline.

**Deploy for free** on Aiven + SnapDeploy, Fly.io, or Koyeb.

---

## Live Demo

| Service | URL |
|---------|-----|
| **API Gateway** | [localhost:8080](http://localhost:8080) |
| **Students Portal** | [localhost:5001](http://localhost:5001) |
| **Grades Portal** | [localhost:5002](http://localhost:5002) |
| **Seq (Logs)** | [localhost:5341](http://localhost:5341) |
| **Prometheus** | [localhost:9090](http://localhost:9090) |
| **Grafana** | [localhost:3000](http://localhost:3000) |
| **RabbitMQ** | [localhost:15672](http://localhost:15672) |

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

**That's it.** Auto-migrations, data seeding (20 students, 5 teachers, 5 classes, 20 grades, notifications), and all 8 containers start automatically.

---

## Architecture

```
                          ┌──────────────────────┐
                          │     API Gateway       │
                          │   YARP Reverse Proxy  │
                          │      Port 8080        │
                          └──────────┬───────────┘
                                     │
                    ┌────────────────┴────────────────┐
                    │                                 │
         ┌─────────┴──────────┐          ┌───────────┴──────────┐
         │    students-mvc    │          │     grades-mvc       │
         │  ASP.NET Core 10   │  HTTP    │   ASP.NET Core 10    │
         │  MVC + Identity    │◄────────►│   MVC + Identity     │
         │                    │          │                      │
         │  • Students CRUD   │          │  • Grades CRUD       │
         │  • Teachers CRUD   │          │  • StudentCache      │
         │  • ClassRooms CRUD │          │  • Resilient Client  │
         │  • Attendance      │          │                      │
         │  • Dashboard       │          │                      │
         │  • Reports         │          │                      │
         │  • Notifications   │          │                      │
         │  Port: 5001        │          │  Port: 5002          │
         └────────┬───────────┘          └───────────┬──────────┘
                  │                                  │
                  │        ┌──────────────┐          │
                  │        │   RabbitMQ   │          │
                  ├───────►│  Fanout Bus  │◄─────────┤
                  │        │  Events Bus  │          │
                  │        └──────────────┘          │
                  │                                  │
         ┌────────┴──────────────────────────────────┴──────────┐
         │                  Shared PostgreSQL 16                 │
         │  ┌─────────────┐ ┌────────────┐ ┌────────────────┐  │
         │  │ Identity     │ │ Students   │ │ Teachers       │  │
         │  │ Tables       │ │            │ │ ClassRooms     │  │
         │  │ Notifications│ │ Attendance │ │ StudentClasses │  │
         │  │ Grades       │ │ StudentCache│ │ ClassTeachers  │  │
         │  └─────────────┘ └────────────┘ └────────────────┘  │
         └──────────────────────────────────────────────────────┘

  ┌─────────────────────────────────────────────────────────────┐
  │                   Observability Stack                        │
  │  ┌──────────┐   ┌────────────┐   ┌──────────────┐          │
  │  │   Seq    │   │ Prometheus │   │   Grafana     │          │
  │  │ :5341    │   │  :9090     │   │   :3000       │          │
  │  └──────────┘   └────────────┘   └──────────────┘          │
  └─────────────────────────────────────────────────────────────┘
```

### Key Design Decisions

- **Single Shared Database** (`defaultdb`): Both services share one PostgreSQL instance with EF Core migrations managed by students-mvc
- **RabbitMQ Fanout Exchange** (`school.student.events`): Students-mvc publishes student events; grades-mvc consumes and maintains a local `StudentCache` table as a fallback
- **YARP API Gateway**: Microsoft reverse proxy with active health checks and RoundRobin load balancing
- **Role-Based Access Control**: 4 roles (Admin, Teacher, Student, Parent) with different permissions on each page
- **Resilient HTTP Client**: `StudentsServiceClient` in grades-mvc falls back to `StudentCache` table on HTTP failure

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| **Backend** | ASP.NET Core 10 (MVC) — C# |
| **Frontend** | Razor Views + Bootstrap 5.3 + Bootstrap Icons + Chart.js |
| **Database** | PostgreSQL 16 (shared `defaultdb` on Aiven) |
| **ORM** | Entity Framework Core 10 + Code-First Migrations |
| **Messaging** | RabbitMQ 3 (fanout exchange for student events) |
| **API Gateway** | YARP 2.2.0 (reverse proxy + health checks) |
| **Authentication** | ASP.NET Core Identity (shared cookie) |
| **Logging** | Serilog + Seq (centralized structured logs) |
| **Monitoring** | Prometheus + Grafana (metrics dashboards) |
| **Container** | Docker + Docker Compose |
| **CI/CD** | GitHub Actions (build + test + Docker Hub push) |
| **Testing** | xUnit + Moq + EF Core InMemory (111 tests) |

---

## Features

### Authentication & Authorization

- **ASP.NET Core Identity** with shared auth cookie (`SchoolPortal.Auth`)
- **4 Roles**: Admin (full CRUD), Teacher (view+edit), Student (view only), Parent (view only)
- Role-based UI badges (red/blue/green/yellow) in the navbar
- Conditional action buttons per role
- **Default Accounts**:
  | Role | Email | Password |
  |------|-------|----------|
  | Admin | `admin@school.com` | `Admin@123` |
  | Teacher | `teacher@school.com` | `Teacher@123` |

### Students & Teachers Management

- Full CRUD operations for Students, Teachers, and ClassRooms
- Server-side search and pagination on all Index pages
- Many-to-many relationships: ClassTeacher, StudentClass
- Form validation with server-side and client-side rules

### Attendance Tracking

- Bulk attendance submission by class and date
- Status options: Present, Absent, Late, Excused
- Filter by class, date, and search by student name
- Color-coded status badges (green/red/orange/blue)
- Auto-notifications for absences, late arrivals, and all-present

### Notifications System

- Bell icon in navbar with unread badge (animated pulse)
- Full notifications page with search, pagination, mark read, delete
- Auto-generated notifications for:
  - Student create/update/delete
  - Teacher create/update/delete
  - Attendance submissions (absences, late, all-present)
- 5 seed notifications on first startup
- Notification types: Info, Success, Warning, Error
- Categories: System, Student, Teacher, Attendance, Grade, Class

### Dashboard & Reports

- **Dashboard**: Stats cards, attendance ring chart (SVG), class statistics with progress bars, teacher workload table, monthly attendance bar chart (Chart.js)
- **Reports**:
  - Student Performance (individual student attendance breakdown)
  - Attendance Report (date range + class filter with daily breakdown)
  - Class Report (per-class stats with student lists)

### RabbitMQ Fault Tolerance

- **Publisher**: `RabbitMqStudentMessageBus` publishes `student.created`, `student.updated`, `student.deleted` events to fanout exchange `school.student.events`
- **Consumer**: `StudentEventConsumer` background service in grades-mvc consumes events and updates local `StudentCache` table
- **Fallback**: When students-mvc is unreachable, `StudentsServiceClient` falls back to `StudentCache` data instead of crashing

### API Gateway (YARP)

| Route | Upstream | Load Balancing |
|-------|----------|---------------|
| `/api/students/*` | students-mvc:8080 | RoundRobin |
| `/api/teachers/*` | students-mvc:8080 | RoundRobin |
| `/Teachers/*` | students-mvc:8080 | RoundRobin |
| `/ClassRooms/*` | students-mvc:8080 | RoundRobin |
| `/Attendance/*` | students-mvc:8080 | RoundRobin |
| `/grades/*` | grades-mvc:8080 | RoundRobin |
| `/grades-api/*` | grades-mvc:8080 | RoundRobin |

Active health checks every 15 seconds with automatic failover.

### Observability

- **Serilog**: Structured logging with `Service` property enrichment in all 3 services
- **Seq**: Centralized log aggregation at `http://localhost:5341` (username: `admin`, password: `Admin@123`)
- **Prometheus**: Scraping metrics from all services at `http://localhost:9090`
- **Grafana**: Pre-configured dashboards at `http://localhost:3000` (admin/admin)

### Testing

- **111 unit tests**, all passing
- **students-mvc.Tests** (83 tests):
  - Models: Student, Teacher, ClassRoom, Attendance, PaginatedList, Notification
  - Controllers: StudentsController, TeachersController, AttendanceController
  - ViewComponents, Services
- **grades-mvc.Tests** (28 tests):
  - Models: Grade, StudentCache, Notification
  - Controllers: GradesController
  - Services: StudentsServiceClient (with MockHttpMessageHandler + cache fallback)

---

## Docker Services (8 Containers)

| Service | Image | Port | Health Check |
|---------|-------|------|-------------|
| PostgreSQL | `postgres:16-alpine` | 5432 | `pg_isready` |
| RabbitMQ | `rabbitmq:3-management-alpine` | 5672, 15672 | `rabbitmq-diagnostics ping` |
| Seq | `datalust/seq:latest` | 5341 | — |
| Prometheus | `prom/prometheus:latest` | 9090 | — |
| Grafana | `grafana/grafana:latest` | 3000 | — |
| students-mvc | `school-portal-students-mvc:latest` | 5001 | `curl /health` |
| grades-mvc | `school-portal-grades-mvc:latest` | 5002 | `curl /health` |
| API Gateway | `school-portal-api-gateway:latest` | 8080 | `curl /health` |

All services use Docker health checks with `depends_on: condition: service_healthy` for proper startup ordering.

---

## Project Structure

```
School-Portal/
├── SchoolPortal.slnx                    # Solution file (all 5 projects)
├── docker-compose.yml                   # Full orchestration (8 services)
├── .env                                 # Environment variables
├── .env.example                         # Environment template
├── prometheus.yml                       # Prometheus scrape config
├── api-gateway/                         # YARP Reverse Proxy
│   ├── Program.cs
│   └── Dockerfile
├── students-mvc/                        # Main MVC Application
│   ├── Controllers/
│   │   ├── StudentsController.cs        # CRUD + Search + Pagination
│   │   ├── TeachersController.cs        # CRUD + Search + Pagination
│   │   ├── ClassRoomsController.cs      # CRUD + Search + Pagination
│   │   ├── AttendanceController.cs      # Filter, TakeAttendance, Notifications
│   │   ├── DashboardController.cs       # Stats, Charts, Analytics
│   │   ├── ReportsController.cs         # Performance, Attendance, Class Reports
│   │   ├── NotificationsController.cs   # MVC Page + JSON API
│   │   ├── AccountController.cs         # Identity Login/Register
│   │   └── HomeController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs      # EF Core context (all student tables)
│   ├── Migrations/
│   ├── Messaging/
│   │   ├── RabbitMqConfig.cs
│   │   ├── RabbitMqStudentMessageBus.cs # Publisher (fanout exchange)
│   │   └── StudentEvent.cs
│   ├── Models/
│   │   ├── Student.cs, Teacher.cs, ClassRoom.cs
│   │   ├── ClassTeacher.cs, StudentClass.cs
│   │   ├── Attendance.cs
│   │   ├── Notification.cs
│   │   ├── PaginatedList.cs             # Generic paginated list
│   │   └── RegisterViewModel.cs
│   ├── Services/
│   │   ├── INotificationService.cs
│   │   └── NotificationService.cs
│   ├── ViewComponents/
│   │   └── PaginationViewComponent.cs
│   ├── Views/
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml
│   │   │   ├── _NotificationBell.cshtml  # Bell dropdown partial
│   │   │   └── Components/Pagination/
│   │   ├── Students/, Teachers/, ClassRooms/
│   │   ├── Attendance/
│   │   ├── Dashboard/
│   │   ├── Reports/
│   │   └── Notifications/
│   ├── wwwroot/css/site.css
│   └── Dockerfile
├── grades-mvc/                          # Grades Microservice
│   ├── Controllers/
│   │   └── GradesController.cs          # CRUD + Search + Pagination
│   ├── Data/
│   │   └── GradesDbContext.cs           # Grades + StudentCache tables
│   ├── Messaging/
│   │   └── StudentEventConsumer.cs      # RabbitMQ Consumer
│   ├── Models/
│   │   ├── Grade.cs
│   │   └── StudentCache.cs              # Local student data replica
│   ├── Services/
│   │   ├── IStudentsServiceClient.cs
│   │   └── StudentsServiceClient.cs     # HTTP client with cache fallback
│   └── Dockerfile
├── students-mvc.Tests/                  # 83 unit tests
│   ├── TestDbHelper.cs
│   ├── Controllers/                     # Students, Teachers, Attendance
│   ├── Models/                          # Student, Teacher, ClassRoom, etc.
│   └── Services/
├── grades-mvc.Tests/                    # 28 unit tests
│   ├── GradesTestDbHelper.cs
│   ├── Controllers/GradesControllerTests.cs
│   ├── Models/
│   └── Services/StudentsServiceClientTests.cs
├── .github/workflows/ci-cd.yml         # GitHub Actions CI/CD
├── DEPLOYMENT.md                        # Free deployment guide
├── LICENSE                              # MIT License
└── README.md
```

---

## Free Deployment

| Platform | Free Tier | Guide |
|----------|-----------|-------|
| **Aiven + SnapDeploy** | PostgreSQL 1GB + 4 Containers | [DEPLOYMENT.md](DEPLOYMENT.md) |
| **Aiven + JustRunMy.App** | PostgreSQL 1GB + Docker | [DEPLOYMENT.md](DEPLOYMENT.md) |
| **Fly.io** | 3 VMs + 3GB storage | [DEPLOYMENT.md](DEPLOYMENT.md) |
| **Koyeb** | 1 free nano service | [DEPLOYMENT.md](DEPLOYMENT.md) |

---

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | `postgres` |
| `POSTGRES_PASSWORD` | PostgreSQL password | `YourStrongPassword123` |
| `ADMIN_PASSWORD` | Admin user password | `Admin@123` |
| `RABBITMQ_USER` | RabbitMQ username | `guest` |
| `RABBITMQ_PASS` | RabbitMQ password | `guest` |
| `StudentsService__BaseUrl` | Students service URL (Docker) | `http://students-mvc:8080` |
| `Serilog__SeqServer` | Seq logging server | `http://seq:5341` |

---

## Development

### Prerequisites

- Docker Desktop
- .NET 10 SDK (for local development and running tests)

### Run Tests

```bash
# Run all 111 tests
dotnet test SchoolPortal.slnx
```

### Build Docker Images

```bash
# Build and start all 8 containers
docker compose up --build

# Check container health
docker compose ps
```

### Git History

```
6417cc8  fix: DateTime.UtcNow, IPaginatedList, duplicate Scripts section
d1e2d12  feat: Notifications system (bell, page, auto-events, seed data)
bcc387c  feat: Unit tests (111 tests) + CI/CD pipeline
0fe763f  feat: Search + Pagination on all controllers
c54e348  feat: Logging (Serilog + Seq + Prometheus + Grafana)
b2536ff  feat: Dashboard + Reports (Chart.js, SVG charts)
22bb2de  feat: API Gateway (YARP) + Docker health checks
0d01871  feat: RabbitMQ fault tolerance + StudentCache fallback
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
