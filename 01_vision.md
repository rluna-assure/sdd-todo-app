# TODO LIST APP(this app allows how for doing some stories)

You are a senior software architect therefore you will create an app to create only boards and tasks services. 
Inside a board the user can create tickets, update and remove.
The user can create tickets, move to "in-progress" and close when the ticket is "done".
User can add comments where the task tickets where is "in-progress".
This app is just rest services in backend using C# and .Net Core as library.

#### Specs:
 - Create Boards
 - Create Tasks
 - Update status task(open, in-progress, done)
 - Add comments in tasks
 - Logic Delete
 - Add comments in the task

#### Architecture
Follow the next structure:
Controller -> Service -> Data (Controller-Based Web APIs in .NET Core.)
Stack:
 - REST
 - C#
 - .Net core 
 - good practices
 - clean code
 - Open API (endpoint documentation)

### Non-Functional Requirements
 - Validation: The API must validate all inputs and return clear error messages alongside the appropriate HTTP status codes.
 - Security: Initial authentication will not be implemented.
 - Maintainability: Unit testing must be included.
 - Soft Delete: Records must be marked as inactive rather than physically deleted from the database.
 - Documentation: Endpoints must be documented using the OpenAPI Specification to facilitate testing and consumption.
 - No Database should be used. Use memory to store data.
 - Create just the backend services, no UI or integration tests.
 - No need for authentication or authorization.
 - Create Unit tests 
 - Create Endpoints documentation with OpenAPI Specification, make that this API doc easy to understand and use, with examples of request and response.
 - Make sure that the API endpoints are well organized and easy to understand.
 - Always create the code with the best practices and clean code, following the best practices and clean code, Solid principles, KISS, YAGNI, DRY, etc.
 - generate all endpoints documentation using OpenAPI Specification and save in 05_openapi.yml file.
 - generate all Unit tests and save in 04_unit-tests.md file.
 