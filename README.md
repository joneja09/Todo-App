# To-Do App

A task management application with a .NET 9 backend (single project, Microsoft Identity, EF Core with SQLite) and two frontends (React, Vue). Supports multiple users with multiple to-do lists via Bearer authentication. Backend is deployed via GitHub Actions to Azure App Service, frontends are automatically build/deployed by Vercel when commit to the `main` branch happens in the repository.

## Project Structure

- `backend/TodoApp/`: .NET 9 API with folders structuring represents the Onion Architecture, using ASP.NET Core Identity and EF Core with SQLite. Dependency injection is 
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
4. Run `dotnet ef database update` to create SQLite DB. (Optional: migrations will apply on startup of the api)
5. Run `dotnet run` (starts at `https://localhost:58273` with Swagger at `/swagger`).

### Frontend (React)

1. Navigate to `frontend`.
2. Run `yarn install`.
3. Create and Set `REACT_APP_API_URL=https://localhost:58273` in `frontend/.env`.
4. Run `yarn start` (starts at `http://localhost:3000`).

### Frontend (Vue)

1. Navigate to `frontend-vue`.
2. Run `yarn install`.
3. Create and Set `VITE_API_URL=https://localhost:58273` in `frontend-vue/.env`.
4. Run `yarn dev` (starts at `http://localhost:5173`).

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
  - Push to `main` deploys the frontend projects.  Vercel handles this with reference to the repository.
  - _Note_: Can be found at: (React)`https://joneja09-todo-app.vercel.app/` and (Vue)`https://joneja09-todo-app-vue.vercel.app/`

## Usage

1. Register/login via frontend or API (`POST /register` or `/login`).
2. Create to-do lists (`POST /api/todolists`).
3. Add tasks to a list (`POST /api/tasks` with `todoListId`).
4. View/edit/delete tasks (`GET /api/tasks/list/{todoListId}`, `PUT /api/tasks/{id}`, `DELETE /api/tasks/{id}`).
5. View/edit/delete to-do lists (`GET /api/todolists`, `PUT /api/todolists/{id}`, `DELETE /api/todolists/{id}`).

## Assumptions and Trade-offs

- **Single Project**: Backend uses a single .NET project for simplicity, maintaining folder structure for organization.
- **Microsoft Identity**: Uses ASP.NET Core Identity for robust user management, with Bearer for frontend compatibility.  If more complex scenarios were needed look to utilize an Auth provider (eg. Auth0, Duende)
- **Auth**: Bearer-based, refresh tokens usage not implemented.  For practical production application, the refresh token would get used to retreive a new access token as it nears the expiration or when a call is made that returns a 401.
- **DB**: SQLite for simplicity; consider PostgreSQL or Azure SQL for scale and production use.
- **DB Migrations**: SQLite is controlled by the application, so migrations happen on startup; if ported to PostgreSQL or Azure SQL move migrations to GitHub Actions using CLI Command.
- **Data Model**: Users own multiple `TodoList`s, each containing multiple `TaskItem`s, stored in SQLite for easy setup.
- **Tests**: Basic xUnit coverage; aim for 80%+ in production.

## Future Improvements
- Utilize refresh tokens to provide a better user experience.
- Utilize Azure SQL to have a persisted reliable database.
- Add integration/E2E tests (Playwright/Cypress) ton ensure quality frontend.
- Implement task sharing or team capability, notifications (WebSockets).
- Add Redis Caching/Output Caching for high scale.
- Containerize with Docker for consistent contained deployment.
