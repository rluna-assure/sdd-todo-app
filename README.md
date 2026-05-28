# Todo App Board and Task REST API
This is an app for creating boards, tasks, and comments. Designed under Clean Code and SOLID principles using ASP.NET Core 9.

---
# Features
* **Boards Management:** Full CRUD capabilities with strict title validations and soft-deactivation mechanisms.
* **Tasks Management:** Full CRUD capabilities with strict title validations and soft-deactivation mechanisms.
* **Comments Management** Comments can only be added, updated, or deleted when task is in **InProgress**.
* **Soft Delete:** Logical deletion flag (`IsActive`) applied across boards, tasks, and comments to preserve transaction history and maintain audit logs.
---
## Spec-Driven Development (SDD) Workflow
This project was built using **Spec-Driven Development (SDD)**. SDD prioritizes the architectural planning, creating a highly structured path for both human developers and AI agents.

### The SDD Implementation Process:
1. **Requirement & Vision Mapping:**
   I defined the product scope and technical boundaries (in-memory thread-safe collections, l architecture, transition rules) inside [00_initial_requeriments.md](docs/00_initial_requeriments.md) and [01_vision.md](docs/01_vision.md).
2. **Behavioral Specification (User Stories):**
   We authored formal functional scenarios inside [02_user-stories.md](docs/02_user-stories.md) to serve as definitive acceptance criteria.
3. **Data & Routing Design:**
   Designed exact data models in [03_data-model.md](docs/03_data-model.md) and exposed RESTful routing through the comprehensive **05_openapi.yml** contract.

---
## Getting Started

### Prerequisites
 [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
### Running the API
Run the Web API project using the .NET CLI:
```bash
dotnet run --project todo-app/todo-app
```

---
## Verification & Automated Tests
To execute all automated unit tests, navigate to the solution root and run:
```bash
dotnet test
```
### Covered Scenarios:
* **Board Operations:** Board creation, title/description validation, and soft-delete states (deactivating boards and query filtering).
* **Task Workflow:** Status transition validations (`Open` -> `InProgress`, `InProgress` -> `Done` and exceptions thrown on out-of-order/backward transitions).
* **Comment Restrictions:** Success paths for adding comments during `InProgress` task status, and rejection validation (returns error on `Open` or `Done` tasks).
