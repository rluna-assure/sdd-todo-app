### PROMPTS USED

#### 1 - Antigravity Editor
	- Model Used: Gemini 3 Flash
	- Prompt Template: "please procced and generate all mentioned in the project"


#### 2- Results
# Implementation Plan - C# .NET Core Web API Todo List App

This plan details the design, architecture, and step-by-step implementation for the Backend REST API of the Todo List App using C# and .NET Core 9.

## User Review Required

> [!IMPORTANT]
> Since we are using an **In-Memory Data Store** as specified, all data will be cleared when the application restarts. To ensure thread safety, all operations will utilize thread-safe collections (`ConcurrentDictionary`, thread synchronization locks, etc.).

> [!NOTE]
> **API Routing Strategy & Clarifications**:
> To resolve the ambiguous routes in `04_openapi.md` where both Board and Task operations mapped to identical routes like `api/v1/get/{id}`, we propose the following clear, conventional, and RESTful routing scheme:
> - **Boards Controllers Base Route**: `api/v1/boards`
>   - `GET /api/v1/boards/{id}` - Get a Board by ID.
>   - `POST /api/v1/boards` - Create a Board (Body: `{ title, description }`).
>   - `PATCH /api/v1/boards/{id}` - Update Board details (Body: `{ title, description }`).
>   - `POST /api/v1/boards/{id}/status` - Enable/Disable Board (Body: `{ isActive: bool }`).
> - **Tasks Controllers Base Route**: `api/v1/tasks`
>   - `GET /api/v1/tasks/{id}` - Get a Task by ID.
>   - `POST /api/v1/tasks` - Create a Task (Body: `{ title, description, boardId }`).
>   - `PATCH /api/v1/tasks/{id}` - Update Task title and description (Body: `{ title, description }`).
>   - `POST /api/v1/tasks/{id}/status` - Update status of a Task (Body: `{ status: string }` where status is `OPEN`, `IN-PROGRESS`, or `DONE`).
>   - `POST /api/v1/tasks/{id}/comment` - Add a comment to a task (Allowed only when task is `IN-PROGRESS`).
>   - `GET /api/v1/tasks/{id}/comments` - Get all comments for a specific task.
>   - `DELETE /api/v1/tasks/comments/{commentId}` - Soft-delete/Remove a comment (Allowed only when task is `IN-PROGRESS`).
>   - `PATCH /api/v1/tasks/comments/{commentId}` - Update a comment (Allowed only when task is `IN-PROGRESS`).
>   - `GET /api/v1/boards/{boardId}/comments` - Get all comments in a Board where tickets are `IN-PROGRESS`.

## Proposed Project Structure

We will create a multi-project solution structure to isolate the Web API implementation from the unit tests:

- `TodoApp.sln` (Solution File)
- `TodoApp.Api/` (Controller-Based Web API)
  - `Controllers/` (BoardsController, TasksController)
  - `Services/` (IBoardService, ITaskService, BoardService, TaskService)
  - `Data/` (InMemoryDbContext, IRepository, InMemoryRepository)
  - `Models/` (Board, Task, Comment, TaskComment, TaskStatus enum)
  - `DTOs/` (Requests/Responses DTOs for structured payload validation)
  - `Program.cs` (Configuration for Dependency Injection, OpenAPI Swagger, and Routing)
- `TodoApp.Tests/` (Unit Tests)
  - `Services/` (BoardServiceTests, TaskServiceTests)
  - `Controllers/` (BoardsControllerTests, TasksControllerTests)

---

## Proposed Changes

### [Component Name] TodoApp Solution & API Project

#### [NEW] [TodoApp.sln](file:///d:/SDD/todo-app/TodoApp.sln)
The central solution file grouping `TodoApp.Api` and `TodoApp.Tests`.

#### [NEW] [TodoApp.Api.csproj](file:///d:/SDD/todo-app/TodoApp.Api/TodoApp.Api.csproj)
ASP.NET Core Web API project targeting .NET 9.0, using controller-based patterns and configuring Swagger/OpenAPI.

#### [NEW] [Models & Enums](file:///d:/SDD/todo-app/TodoApp.Api/Models)
- `Board.cs`: `Id`, `Title`, `Description`, `IsActive` (soft delete indicator).
- `TaskStatus.cs`: Enum with values `Open`, `InProgress`, `Done`.
- `Task.cs`: `Id`, `Title`, `Description`, `IsActive` (soft delete indicator), `BoardId`, `Status` (enum), `OpenDate`, `CloseDate`.
- `Comment.cs`: `Id`, `CommentText`, `DatePost`, `IsActive`.
- `TaskComment.cs`: Junction class holding `Id`, `CommentId`, `TaskId`.

#### [NEW] [Data Layer (In-Memory Database)](file:///d:/SDD/todo-app/TodoApp.Api/Data)
- `InMemoryDbContext.cs`: A thread-safe, single-instance memory store using `ConcurrentBag` or locked generic lists to simulate DB tables.
- `IRepository.cs` & `InMemoryRepository.cs`: Implementation of Generic Repository Pattern to manage CRUD operations with soft delete filters.

#### [NEW] [Services Layer](file:///d:/SDD/todo-app/TodoApp.Api/Services)
- `IBoardService.cs` & `BoardService.cs`: Business logic for Boards (validation, enabling/disabling, CRUD).
- `ITaskService.cs` & `TaskService.cs`: Business logic for Tasks & Comments. Ensures rules like:
  - Transition flow (Open -> InProgress -> Done).
  - Comments can only be added, updated, or deleted when a task is `IN-PROGRESS`.
  - Soft-deletion of boards hides their tasks.

#### [NEW] [DTOs Layer](file:///d:/SDD/todo-app/TodoApp.Api/DTOs)
Structured models for request validation using `System.ComponentModel.DataAnnotations` (e.g., `Required`, `StringLength`).

#### [NEW] [Controllers Layer](file:///d:/SDD/todo-app/TodoApp.Api/Controllers)
- `BoardsController.cs`: Board endpoints with validation, standard status codes (`201 Created`, `200 OK`, `400 BadRequest`, `404 NotFound`).
- `TasksController.cs`: Task & Comment endpoints with validation and error-handling.

#### [NEW] [05_openapi.yml](file:///d:/SDD/todo-app/05_openapi.yml)
A complete, detailed OpenAPI 3.0 specification file document representing all our endpoints with clear request/response models and description examples.

### [Component Name] TodoApp Unit Tests Project

#### [NEW] [TodoApp.Tests.csproj](file:///d:/SDD/todo-app/TodoApp.Tests/TodoApp.Tests.csproj)
Test project using **xUnit** and **Moq** (or in-memory repository tests) to verify business logic and controller endpoints.

#### [NEW] [Service Tests](file:///d:/SDD/todo-app/TodoApp.Tests/Services)
- `BoardServiceTests.cs`: Validates creation, updates, and soft deletes.
- `TaskServiceTests.cs`: Validates status transitions (e.g., Open -> InProgress -> Done) and comments rules (only allowed in InProgress state).

---

## Verification Plan

### Automated Tests
We will implement and run unit tests. We will use the dotnet CLI tools to run tests:
- `dotnet test` within the root workspace.

### Manual Verification
We will start the API locally and check standard Swagger UI endpoints or compile the project to ensure 100% build compatibility.
- `dotnet run --project TodoApp.Api` to start the API server locally.
- Visual inspection of the OpenAPI Specification file to verify format accuracy.

# Project Walkthrough - Todo App REST API

We have fully implemented a production-quality, controller-based C# .NET Core 9 REST Web API for the Todo App inside the workspace. The solution adheres strictly to all functional and non-functional specifications.

---

## Accomplishments

### 1. Project Initialization & Architecture setup
- Created `TodoApp.sln` solution file.
- Created `TodoApp.Api/` project: A Controller-based Web API targeting .NET Core 9.
- Created `TodoApp.Tests/` project: An xUnit test suite referencing the API project.
- Organized the code following clean architecture patterns: **Controller -> Service -> Data**.

### 2. Core Data & Repository Layer
- Created domain models in `Models/`: `Board.cs`, `Task.cs`, `Comment.cs`, `TaskComment.cs`, and `TaskStatus.cs` enum.
- Created `InMemoryDbContext.cs`: A completely thread-safe singleton context simulating DB tables with locked list collections.
- Implemented `IRepository.cs` & `InMemoryRepository.cs`: A generic repository pattern that handles logical/soft deletes automatically via reflection when objects contain an `IsActive` field.

### 3. Business Service Layer
- Implemented `BoardService.cs` & `IBoardService.cs` handling boards logic and soft-deletes.
- Implemented `TaskService.cs` & `ITaskService.cs` containing all task-related restrictions:
  - **Status Transitions**: Task lifecycle strictly moves from `Open` -> `InProgress` -> `Done`. Transitions bypassing intermediate states (e.g. Open -> Done) or going backwards (e.g. InProgress -> Open) are blocked with `InvalidOperationException`.
  - **Task Timing**: Setting status to `Open` initializes the opening date; moving to `Done` logs the task completion date.
  - **Comment Constraints**: Comments can only be added, updated, or soft-deleted when the task is actively `InProgress`. Managing comments in `Open` or `Done` tasks throws exceptions.

### 4. DTOs & Validation Controllers
- Created DTO request models under `DTOs/` with built-in .NET `DataAnnotations` validation attributes (`[Required]`, `[StringLength]`, `[RegularExpression]`).
- Created `BoardsController.cs` & `TasksController.cs` exposing clean RESTful endpoints mapping.
- Cleaned up default project boilerplate files (e.g., deleted weather forecast mocks).

### 5. API Documentation & OpenAPI Specification
- Implemented the complete OpenAPI 3.0 YAML definition inside [05_openapi.yml](file:///d:/SDD/todo-app/05_openapi.yml), including detailed endpoint descriptions, query parameters, request bodies, error schemas, and response mock examples.

---

## Verification Results

We verified the business logic through a comprehensive, automated test suite in `TodoApp.Tests` comprising **12 unit tests** covering all edge cases.

### Automated Test Results
Running `dotnet test` succeeded with 100% pass rate:
```text
Test run for D:\SDD\todo-app\TodoApp.Tests\bin\Debug\net9.0\TodoApp.Tests.dll (.NETCoreApp,Version=v9.0)
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    12, Skipped:     0, Total:    12, Duration: 93 ms - TodoApp.Tests.dll (net9.0)
```

The tests verified:
- Correct creation of boards and tasks.
- Soft-deletion activation filtering (hiding tasks and comments under inactive parents).
- Proper transition flows (validating `Open` -> `InProgress` -> `Done` and throwing exceptions on invalid workflows).
- Strict Commenting constraints (only allowing edits/deletions/creations while task is `InProgress`).
