# To-Do App

A task management application with a .NET 9 backend (single project, Microsoft Identity, EF Core with SQLite) and two frontends (React, Vue). Supports multiple users with multiple to-do lists via JWT authentication. Deployed via GitHub Actions: backend to Azure App Service, frontends to Vercel.

## Project Structure

- `backend/TodoApp/`: .NET 9 API with folders for `Entities`, `Interfaces`, `DTOs`, `Responses`, `Services`, `Persistence`, `Repositories`, `Controllers`, using ASP.NET Core Identity and EF Core with SQLite.
- `backend/TodoApp.Tests/`: xUnit tests for services and controllers.
- `frontend/`: React frontend with TypeScript, supports multiple to-do lists.
- `frontend-vue/`: Vue 3 frontend with TypeScript, supports multiple to-do lists.
- `.github/workflows/ci.yml`: CI/CD pipeline (builds, tests, deploys).

## Prerequisites

- .NET 9 SDK
- Node.js 20+
- Yarn (install globally: `npm install -g yarn`)
- Git

## Setup Instructions

### Backend

1. Navigate to `backend/TodoApp`.
2. Run `dotnet restore`.
3. Run `dotnet ef migrations add InitialCreate` to create migration for Identity and app entities.
4. Run `dotnet ef database update` to create SQLite DB.
5. Run `dotnet run` (starts at `https://localhost:58273` with Swagger at `/swagger`).

### Frontend (React)

1. Navigate to `frontend`.
2. Run `yarn install`.
3. Run `yarn start` (starts at `http://localhost:3000`).
4. Set `REACT_APP_API_URL=https://localhost:5001/api` in `frontend/.env`.

### Frontend (Vue)

1. Navigate to `frontend-vue`.
2. Run `yarn install`.
3. Run `yarn dev` (starts at `http://localhost:5173`).
4. Set `VITE_API_URL=https://localhost:5001/api` in `frontend-vue/.env`.

### Tests

- Backend: Run `dotnet test` in `backend/TodoApp.Tests`.
- Frontend: Manual testing; add Jest later for automated tests.

## Deployment

- **Backend (Azure App Service)**:
  - Create a free App Service in Azure Portal.
  - Add GitHub secrets: `AZURE_WEBAPP_NAME`, `AZURE_PUBLISH_PROFILE` (from Azure > App Service > Deployment Center).
  - Update `appsettings.json` `DefaultConnection` to a server-based SQLite path or use Azure SQL/PostgreSQL for production.
  - Push to `main` triggers deploy via `.github/workflows/ci.yml`.
  - _Note_: Can be found at: `https://joneja09-todo-bcgwamedc9cvbyg8.centralus-01.azurewebsites.net`
- **Frontend (Vercel)**:
  - Import `frontend` and `frontend-vue` to Vercel as separate projects.
  - Add GitHub secrets: `VERCEL_TOKEN`, `VERCEL_ORG_ID`, `VERCEL_PROJECT_ID_REACT`, `VERCEL_PROJECT_ID_VUE`.
  - Set Vercel env vars: `REACT_APP_API_URL` and `VITE_API_URL` to Azure backend URL (e.g., `https://your-app.azurewebsites.net/api`).
  - Push to `main` deploys frontends.
  - _Note_: Can be found at: (React)`https://joneja09-todo-app.vercel.app/` and (Vue)`https://joneja09-todo-app-vue.vercel.app/`

## Usage

1. Register/login via frontend or API (`POST /register` or `/login`).
2. Create to-do lists (`POST /api/todolists`).
3. Add tasks to a list (`POST /api/tasks` with `todoListId`).
4. View/edit/delete tasks (`GET /api/tasks/list/{todoListId}`, `PUT /api/tasks/{id}`, `DELETE /api/tasks/{id}`).
5. View/edit/delete to-do lists (`GET /api/todolists`, `PUT /api/todolists/{id}`, `DELETE /api/todolists/{id}`).
6. Access deployed app via Vercel URLs and Azure backend.

## Assumptions and Trade-offs

- **Single Project**: Backend uses a single .NET project for simplicity, maintaining folder structure for organization.
- **Microsoft Identity**: Uses ASP.NET Core Identity for robust user management, with Bearer for frontend compatibility.
- **Data Model**: Users own multiple `TodoList`s, each containing multiple `TaskItem`s, stored in SQLite for easy setup.
- **Auth**: Bearer-based, refresh tokens usage not implemented.
- **DB**: SQLite for simplicity; consider PostgreSQL or Azure SQL for scale.
- **Azure Free Tier**: Limited to 60 CPU min/day, may sleep after inactivity.
- **Vercel**: Unlimited bandwidth, auto-HTTPS.
- **Tests**: Basic xUnit coverage; aim for 80%+ in production.

## Future Improvements

- Add refresh tokens, HTTPS enforcement.
- Add integration/E2E tests (Playwright/Cypress).
- Implement role-based auth, task sharing, notifications (WebSockets).
- Add Redis caching, microservices for high scale.
- Containerize with Docker for consistent deployment.
