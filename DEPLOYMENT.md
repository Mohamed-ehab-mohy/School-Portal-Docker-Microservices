# Deployment Guide

## Free Deployment Options

### Option 1: Render.com (Recommended)

Render offers a generous free tier with PostgreSQL support.

**Steps:**

1. **Fork** the repository on GitHub
2. Go to [render.com](https://render.com) and sign up with GitHub
3. **Create PostgreSQL Database:**
   - Click "New" → "PostgreSQL"
   - Name: `school-portal-db`
   - Plan: **Free**
   - Click "Create Database"
   - Copy the **Internal Database URL**

4. **Deploy students-mvc:**
   - Click "New" → "Web Service"
   - Connect your GitHub repo
   - Name: `school-portal-students`
   - Runtime: **Docker**
   - Dockerfile Path: `./students-mvc/Dockerfile`
   - Plan: **Free**
   - Add Environment Variables:
     ```
     ConnectionStrings__DefaultConnection = <paste Internal Database URL>
     ASPNETCORE_ENVIRONMENT = Production
     ```
   - Click "Create Web Service"

5. **Deploy grades-mvc:**
   - Click "New" → "Web Service"
   - Connect your GitHub repo
   - Name: `school-portal-grades`
   - Runtime: **Docker**
   - Dockerfile Path: `./grades-mvc/Dockerfile`
   - Plan: **Free**
   - Add Environment Variables:
     ```
     ConnectionStrings__DefaultConnection = <paste Internal Database URL>
     StudentsService__BaseUrl = https://school-portal-students.onrender.com
     ASPNETCORE_ENVIRONMENT = Production
     ```
   - Click "Create Web Service"

6. **Wait for deployment** (first deploy takes 5-10 minutes)

**Free Tier:** 750 hours/month per service

**Access URLs:**
- Students Portal: `https://school-portal-students.onrender.com`
- Grades Portal: `https://school-portal-grades.onrender.com`

---

### Option 2: Fly.io

Fly.io offers a free tier with 3 shared-cpu VMs.

**Steps:**

1. Install Fly CLI:
   ```bash
   curl -L https://fly.io/install.sh | sh
   ```

2. Sign up:
   ```bash
   fly auth signup
   ```

3. Create apps:
   ```bash
   # Students service
   cd students-mvc
   fly launch --name school-portal-students
   fly postgres create --name school-portal-db
   fly secrets set ConnectionStrings__DefaultConnection="<database-url>"

   # Grades service
   cd ../grades-mvc
   fly launch --name school-portal-grades
   fly secrets set ConnectionStrings__DefaultConnection="<database-url>"
   fly secrets set StudentsService__BaseUrl="https://school-portal-students.fly.dev"
   ```

4. Deploy:
   ```bash
   fly deploy
   ```

**Free Tier:** 3 shared-cpu-1x VMs + 3GB storage

---

### Option 3: Koyeb

Koyeb offers a free tier with Docker support.

**Steps:**

1. Go to [koyeb.com](https://koyeb.com) and sign up
2. Create a PostgreSQL database
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
