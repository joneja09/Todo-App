# To-Do App

A task management application with a .NET 9 backend (single project, Microsoft Identity, EF Core with SQLite) and two frontends (React, Vue). Supports multiple users with multiple to-do lists via Bearer authentication. Backend is deployed via GitHub Actions to Azure App Service, frontends are automatically build/deployed by Vercel when commit to the `main` branch happens in the repository.

## Project Structure

- `backend/TodoApp/`: .NET 9 API with folders structuring represents the Onion Architecture, using ASP.NET Core Identity and EF Core with SQLite.
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
  - Push to `main` deploys the frontend projects.  Vercel handles this automatically with reference to the repository.
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

## Backend Architecture

### Onion Architecture
Onion Architecture provides clear separation of concerns and maintains dependency inversion, even within a single .NET project.

### Key Benefits:
1. **Dependency Inversion:** The core business logic (Services) depends only on interfaces, not concrete implementations. For example, TaskService depends on ITaskRepository, not the concrete TaskRepository class.

2. **Testability:** Each layer can be easily unit tested in isolation. The Services layer contains pure business logic without infrastructure dependencies, making it highly testable.

3. **Maintainability:** The folder structure clearly represents the architectural layers:
    - Entities/ - Core domain models
    - Interfaces/ - Contracts/abstractions
    - Services/ - Business logic layer
    - Repositories/ - Data access layer
    - Controllers/ - Presentation layer
    - DTOs/ - Data transfer contracts

4. **Flexibility:** If there is a need to switch from SQLite to PostgreSQL or add caching, only the Repository implementations need to change without touching business logic.

### Trade-offs:
- **Single Project Approach:** Instead of multiple projects for each layer, I used folder organization within one project for simplicity and faster development cycles.
- **Simplified Domain:** For this Todo app, complex domain logic wasn't needed, but the architecture supports future complexity.
- **Direct DI Registration:** Used simple dependency injection in Program.cs rather than more complex registration patterns (Extension Methods, Assembly Scanning, etc)

### Why Not Other Architectures:
- **N-Layer Architecture:** Would create tight coupling between layers and make testing harder
- **Clean Architecture:** Similar benefits but more complex setup - Onion Architecture provides the right balance for this scope
- **Microservices:** Overkill for a Todo app's complexity level

This architecture ensures the application is maintainable, testable, and ready to scale as requirements grow, while keeping the implementation pragmatic for the current scope.

### Technical Implementation Details:

**Data Access Layer:**
- **Entity Framework Core**: Code-first approach with SQLite provider for development simplicity
- **Identity Integration**: Extends `IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>` for user management
- **Relationship Mapping**: Explicit foreign key relationships using Fluent API in `OnModelCreating()`
- **Async Operations**: All data operations use async/await pattern for better performance

**API Design Patterns:**
- **Consistent Response Format**: Standardized `ApiResponse<T>` wrapper for all endpoints with success/error handling
- **DTO Pattern**: Clean separation between domain entities and API contracts (TaskDto, TodoListDto, UserDto)
- **RESTful Conventions**: Proper HTTP methods and status codes following REST principles
- **Authorization Policy**: Custom "api" policy using Bearer token authentication scheme

**Cross-Cutting Concerns:**
- **CORS**: Configured for cross-origin requests to support frontend applications
- **Swagger/OpenAPI**: Integrated API documentation available at `/swagger` in development
- **Database Migrations**: Automatic migration application on startup for deployment simplicity
- **Dependency Injection**: Constructor injection with scoped lifetimes for services and repositories

**Security & Authentication:**
- **ASP.NET Core Identity**: Built-in user management with customized integer primary keys
- **Bearer Token Authentication**: JWT-based stateless authentication for SPA compatibility
- **Authorization Attributes**: Controller-level authorization with policy-based access control

## Frontend Architecture

Both frontends implement the same feature set with different frameworks and patterns:

**React Frontend (`frontend/`):**
- **Tech Stack**: React 18, TypeScript, Create React App, Axios
- **State Management**: React hooks (useState) with localStorage persistence
- **Styling**: Custom CSS with dark/light theme support
- **Notifications**: react-hot-toast for user feedback
- **Structure**: Component-based architecture with shared API service layer

**Vue Frontend (`frontend-vue/`):**
- **Tech Stack**: Vue 3 Composition API, TypeScript, Vite, Axios
- **State Management**: Pinia stores (`useAuthStore`, `useTaskStore`) with centralized state
- **Styling**: Custom CSS with dark/light theme support (shared styles with React)
- **Notifications**: vue-toastification for user feedback
- **Structure**: Composition API with reactive stores for state management

**Shared Features:**
- Bearer token authentication with automatic header injection
- Dark/light theme toggle with localStorage persistence
- Responsive design with FontAwesome icons
- Complete CRUD operations for TodoLists and Tasks
- Real-time state synchronization with backend API
- Environment-based API URL configuration (`.env` files)