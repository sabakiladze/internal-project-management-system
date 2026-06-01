```
A RESTful API built with ASP.NET Core 8 designed for internal company management. The system allows companies to manage their projects, employees, and tasks in one place.

## Technologies & Packages
- **ASP.NET Core 8** — Web API framework
- **Entity Framework Core** — Database ORM (SQL-first approach)
- **SQL Server** — Database
- **JWT Bearer** — Authentication and authorization
- **AutoMapper** — Object-to-object mapping
- **BCrypt.Net-Next** — Password hashing
- **Data Annotations** — Request validation
- **Swagger UI** — API documentation and testing

## Authentication
- `POST /api/auth/register` — Register new user with role assignment
- `POST /api/auth/login` — Login and receive JWT Access Token and Refresh Token
- `POST /api/auth/refresh` — Refresh expired Access Token using Refresh Token

## Role Management
- `GET /api/role` — Get all roles (Authorized)
- `GET /api/role/{id}` — Get role by ID (Authorized)
- `POST /api/role` — Create new role (Admin only)
- `PUT /api/role/{id}` — Update role (Admin only)
- `DELETE /api/role/{id}` — Delete role (Admin only)

## Project Management
- `GET /api/project` — Get all projects (Admin, Manager)
- `GET /api/project/{id}` — Get project with members (Admin, Manager)
- `GET /api/project/my-projects` — Get current user's projects (All)
- `GET /api/project/{projectId}/members` — Get project members (Admin, Manager)
- `POST /api/project` — Create project (Admin, Manager)
- `PUT /api/project/{id}` — Update project (Admin, Manager)
- `DELETE /api/project/{id}` — Delete project (Admin only)
- `POST /api/project/{projectId}/members/{userId}` — Add member to project (Admin, Manager)
- `DELETE /api/project/{projectId}/members/{userId}` — Remove member from project (Admin, Manager)

## Task Management
- `GET /api/tasks` — Get all tasks (Admin, Manager)
- `GET /api/tasks/{id}` — Get task by ID (Authorized)
- `GET /api/tasks/my-tasks` — Get current user's tasks (All)
- `GET /api/tasks/project/{projectId}` — Get tasks by project (Admin, Manager)
- `POST /api/tasks/project/{projectId}` — Create task in project (Admin, Manager)
- `PUT /api/tasks/{id}` — Update task (Admin, Manager)
- `PUT /api/tasks/{id}/status` — Change task status (All)
- `PUT /api/tasks/{taskId}/assign/{userId}` — Assign user to task (Admin, Manager)
- `PUT /api/tasks/{taskId}/remove-user` — Remove user from task (Admin, Manager)
- `DELETE /api/tasks/{id}` — Delete task (Admin only)

## User Management
- `GET /api/user` — Get all users (Admin, Manager)
- `GET /api/user/{id}` — Get user by ID (Admin, Manager)
- `PUT /api/user/{userId}/assign-role/{roleId}` — Assign role to user (Admin, Manager)
- `DELETE /api/user/{userId}/remove-role/{roleId}` — Remove role from user (Admin only)
- `PUT /api/user/{userId}/deactivate` — Deactivate user (Admin only)
```
