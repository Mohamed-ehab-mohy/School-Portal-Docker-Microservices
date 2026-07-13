# Deployment Guide (No Credit Card Required)

## Free Deployment Options

All options below are **100% free** and **no credit card required**.

---

### Option 1: Aiven + SnapDeploy (Recommended)

**Database: Aiven (Free PostgreSQL)**
**App Hosting: SnapDeploy (Free Docker)**

#### Step 1: Create Free PostgreSQL on Aiven

1. Go to [aiven.io](https://aiven.io) and sign up (no credit card needed)
2. Click "Create a service" → Select **PostgreSQL**
3. Plan: **Free** (1GB storage, 1GB RAM)
4. Choose a region closest to you
5. Click "Create service"
6. Copy the **Service URI** (looks like: `postgres://user:pass@host:port/dbname`)

#### Step 2: Deploy on SnapDeploy

1. Go to [snapdeploy.dev](https://snapdeploy.dev) and sign up (no credit card needed)
2. Click "New Container"
3. **Container 1 - Students Service:**
   - Name: `school-portal-students`
   - Image: `mcr.microsoft.com/dotnet/aspnet:10.0` (we'll build from source)
   - Or use: Push your Docker image
   - Port: `8080`
   - Environment Variables:
     ```
     ConnectionStrings__DefaultConnection=<paste Aiven PostgreSQL URI>
     ASPNETCORE_ENVIRONMENT=Production
     ```
   - Click "Deploy"

4. **Container 2 - Grades Service:**
   - Name: `school-portal-grades`
   - Port: `8080`
   - Environment Variables:
     ```
     ConnectionStrings__DefaultConnection=<paste Aiven PostgreSQL URI>
     StudentsService__BaseUrl=https://school-portal-students.snapdeploy.dev
     ASPNETCORE_ENVIRONMENT=Production
     ```
   - Click "Deploy"

#### Step 3: Initialize Database

The first time you deploy, the app will automatically create tables via EF Core Migrations.

**Free Tier:** 4 containers, 512MB RAM each, auto-sleep when idle

---

### Option 2: Aiven + JustRunMy.App

**Database: Aiven (Free PostgreSQL)**
**App Hosting: JustRunMy.App (Free Docker)**

#### Step 1: Create Free PostgreSQL on Aiven
(Same as Option 1, Step 1)

#### Step 2: Deploy on JustRunMy.App

1. Go to [justrunmy.app](https://justrunmy.app) and sign up (no credit card needed)
2. Click "New App"
3. **App 1 - Students Service:**
   - Name: `school-portal-students`
   - Deploy method: **Docker Image**
   - Image: Build from your GitHub repo
   - Port: `8080`
   - Environment Variables:
     ```
     ConnectionStrings__DefaultConnection=<paste Aiven PostgreSQL URI>
     ASPNETCORE_ENVIRONMENT=Production
     ```
   - Click "Deploy"

4. **App 2 - Grades Service:**
   - Name: `school-portal-grades`
   - Port: `8080`
   - Environment Variables:
     ```
     ConnectionStrings__DefaultConnection=<paste Aiven PostgreSQL URI>
     StudentsService__BaseUrl=https://school-portal-students.justrunmy.app
     ASPNETCORE_ENVIRONMENT=Production
     ```
   - Click "Deploy"

**Free Tier:** 0.15 vCPU, 0.25 GB RAM, 0.3 GB Disk per app

---

### Option 3: Fly.io (No Credit Card)

Fly.io offers a free tier without credit card for basic usage.

#### Step 1: Install Fly CLI
```bash
curl -L https://fly.io/install.sh | sh
```

#### Step 2: Create PostgreSQL
```bash
fly postgres create --name school-portal-db --plan free
```

#### Step 3: Deploy Students Service
```bash
cd students-mvc
fly launch --name school-portal-students
fly secrets set ConnectionStrings__DefaultConnection="<database-url>"
fly deploy
```

#### Step 4: Deploy Grades Service
```bash
cd ../grades-mvc
fly launch --name school-portal-grades
fly secrets set ConnectionStrings__DefaultConnection="<database-url>"
fly secrets set StudentsService__BaseUrl="https://school-portal-students.fly.dev"
fly deploy
```

**Free Tier:** 3 shared-cpu-1x VMs, 256MB RAM each, 3GB storage

---

### Option 4: Koyeb (No Credit Card)

Koyeb offers a free tier without credit card.

1. Go to [koyeb.com](https://koyeb.com) and sign up
2. Create a PostgreSQL database (free tier)
3. Deploy each service as a Docker app
4. Set environment variables

**Free Tier:** 1 free nano service + 1 free PostgreSQL database

---

## Local Development

### Prerequisites
- Docker Desktop installed
- .NET 10 SDK (for local development without Docker)

### Quick Start
```bash
# Clone the repository
git clone https://github.com/Mohamed-ehab-mohy/School-Portal-Docker-Microservices.git
cd School-Portal-Docker-Microservices

# Copy environment file
cp .env.example .env

# Start all services
docker compose up --build

# Access the application
# Students Portal: http://localhost:5001
# Grades Portal: http://localhost:5002
```

### Default Credentials
- **Admin Email:** admin@school.com
- **Admin Password:** Admin@123 (change via ADMIN_PASSWORD env var)

---

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | postgres |
| `POSTGRES_PASSWORD` | PostgreSQL password | YourStrongPassword123 |
| `ADMIN_PASSWORD` | Admin user password | Admin@123 |
| `StudentsService__BaseUrl` | Students service URL | http://students-mvc:8080 |

---

## API Endpoints

### Students Service (Port 5001)
- `GET /api/students` - Get all students (Admin/Teacher only)
- `GET /api/students/{id}` - Get student by ID
- `GET /health` - Health check

### Grades Service (Port 5002)
- `GET /health` - Health check
