### Boards 

**US-001**
**User Story:** As a project manager, I want to create a new board via API.
**AC: Create Board successfully.**
**Given:** User wants to create a board.
**When:** A request sent with method **POST** to `/api/v1/boards` containing a valid JSON body matching the `CreateBoardRequest` schema.
**Then:** The response must return status **201 Created** and a JSON object complying with the `Board` schema.

**US-002**
**User Story:** As a project manager, I want the API to validate board creation payloads.
**AC: Prevent board creation on invalid input.**
**Given:** User wants to create a board.
**When:** A request is sent with method **POST** to `/api/v1/boards` with an empty title, missing fields, or invalid body payload.
**Then:** The response must return an error with status **400 Bad Request** and description "Validation or request body error".

**US-003**
**User Story:** As a project manager, I want to retrieve existing boards via API.
**AC: Retrieve active and optionally inactive boards.**
**Given:** User wants to read the available boards.
**When:** A request is sent with method **GET** to `/api/v1/boards`
**Then:** The response must return status **200 OK** along with an array of objects matching the `Board` schema.

**US-004**
**User Story:** As a project manager, I want to update a board's text details via API.
**AC: Update Board successfully.**
**Given:** User wants to modify an existing active board.
**When:** A request is sent with method **PATCH** to `/api/v1/boards/{id}` with a body matching the `UpdateBoardRequest` schema.
**Then:** The response must return the updated board representation with status **200 OK** matching the `Board` schema.

**US-005**
**User Story:** As a project manager, I want to receive a clear error when attempting to update an invalid board.
**AC: Handle update targeting an inexistent board.**
**Given:** User attempts to update a board.
**When:** A request is sent with method **PATCH** to `/api/v1/boards/{id}` with a non-existent or inactive board ID.
**Then:** The response must return status **404 Not Found** with description "Board not found or inactive".

**US-006**
**User Story:** As a project manager, I want to set the logic state (active/inactive) of a board.
**AC: Toggle Board activation status.**
**Given:** User wants to soft-delete or reactivate a board.
**When:** A request is sent with method **POST** to `/api/v1/boards/{id}/status` with a body matching the `SetBoardStatusRequest` schema (setting `isActive` to true or false).
**Then:** The response must return status **200 OK** confirming the status was updated successfully.

**US-007**
User Story: As a project manager, I want to retrieve a specific board's details by its ID.
**AC: Retrieve a single active board successfully.**
**Given:** A board exists and is active.
**When:** A request is sent with method **GET** to `/api/v1/boards/{id}` with a valid board ID.
**Then:** The response must return status **200 OK** and a JSON object matching the `Board` schema, or status **404 Not Found** if the board does not exist or is inactive.

---
### Tasks

**US-008**
**User Story:** As a project manager, I want to add a task by providing its target board context in the body.
**AC: Create Task successfully.**
**Given:** User wants to create a task.
**When:** A request is sent with method **POST** to `/api/v1/tasks` with a body matching the `CreateTaskRequest` schema (which includes the target `boardId`).
**Then:** The response must return status **201 Created** with the initialized task object matching the `Task` schema, showing status as "Open".

**US-009**
**User Story:** As a project manager, I want to block task creation if the target board is missing or inactive.
**AC: Prevent orphan task creation.**
**Given:** User attempts to create a task.
**When:** A request is sent with method **POST** to `/api/v1/tasks` where the `boardId` inside the body does not exist or belongs to an inactive board.
**Then:** The response must reject the operation returning status **400 Bad Request**.

**US-010**
**User Story:** As a project manager, I want to edit a task details via API.
**AC: Update Task text fields successfully.**
**Given:** User wants to update task fields.
**When:** A request is sent with method **PATCH** to `/api/v1/tasks/{id}` with a body matching the `UpdateTaskRequest` schema.
**Then:** The response must persist changes and return status **200 OK** along with the updated `Task` object.

**US-011** 
 **User Story:** As a project manager, I want to transition a task from Open to In-Progress via API to track when work begins.
 **AC: Transition status from Open to InProgress successfully.**
 **Given:** A task currently exists with an `Open` status.
 **When:** A request is sent with method **POST** to `/api/v1/tasks/{id}/status` with an `UpdateTaskStatusRequest` body containing the status value `"InProgress"`.
  **Then:** The system must accept the transition, update the status field, and return status **200 OK**.

 **US-012**
 **User Story:** As a project manager, I want to transition a task from In-Progress to Done via API
 **AC: Transition status from InProgress to Done successfully and log metadata.**
 **Given:** A task currently exists with an `InProgress` status.
 **When:** A request is sent with method **POST** to `/api/v1/tasks/{id}/status` with an `UpdateTaskStatusRequest` body containing the status value `"Done"`.
 **Then:** The system must accept the transition, update the status field, automatically populate the `closeDate` with the current timestamp, and return status **200 OK**.

 **US-013**
  **User Story:** As a project manager, I want the API to enforce a strict sequential workflow status.
  **AC: Block invalid or non-sequential workflow transitions.**
  **Given:** A task exists in any workflow status (`Open`, `InProgress`, or `Done`).
  **When:** A request is sent with method **POST** to `/api/v1/tasks/{id}/status` with an `UpdateTaskStatusRequest` body attempting an illegal transition, such as:
    - Moving straight from `Open` to `Done`   
    - Moving backwards from `InProgress` to `Open`.        
    - Modifying the status of a task that is already `Done`.        
  **Then:** The business logic must completely block the operation, prevent any database changes, and return status **400 Bad Request** with description "Invalid status transition or format".

**US-014**
**User Story:** As a project manager, I want to retrieve a specific task's details by id.
**AC: Retrieve a single active task successfully.**
**Given:** A task exists and is active, and is located in an active board.
**When:** A request is sent with method **GET** to `/api/v1/tasks/{id}` with a valid task ID.
**Then:** The response must return status **200 OK** and a JSON object matching the `Task` schema, or status **404 Not Found** if the task or its parent board does not exist or is inactive.

**US-015**
**User Story:** As a project manager, I want to retrieve all tasks associated with a specific board.
**AC: Retrieve active and optionally inactive tasks for a board successfully.**
**Given:** A board exists and is active.
**When:** A request is sent with method **GET** to `/api/v1/tasks/board/{boardId}` with a valid board ID.
**Then:** The response must return status **200 OK** along with an array of objects matching the `Task` schema, filtering out inactive tasks unless the query parameter `includeInactive` is set to true, or returning status **404 Not Found** if the board does not exist or is inactive.

---
### Comments

**US-016**
**User Story:** As a project manager, I want to add comments exclusively to active tasks that are currently in progress.
**AC:** Control comment creation based on task lifecycle status.
**Given:** A user wants to add a comment to a task.
**When:** A request is sent with method **POST** to `/api/v1/tasks/{id}/comment` with an `AddCommentRequest` body.
**Then:** The task status is `InProgress`, the comment is saved and returns status **200 OK** along with the `Comment` schema object.

**US-017**
**User Story:** As a project manager, I want to add comments exclusively to active tasks that are currently in progress.
**AC:** Control comment creation based on task lifecycle status.
**Given:** A user wants to add a comment to a task.
**When:** A request is sent with method **POST** to `/api/v1/tasks/{id}/comment` with an `AddCommentRequest` body.
**Then:** The task is inactive or has a status of `Open` or `Done`, the operation is blocked returning status **400 Bad Request**.

**US-018**
**User Story:** As a project manager, I want to read all comments related to a specific task.
**AC:** Retrieve task comment collection.
**Given:** Comments exist inside an active task.
**When:** A request is sent with method **GET** to `/api/v1/tasks/{id}/comments`.
**Then:** The response must return status **200 OK** with an array of objects matching the `Comment` schema.

**US-019**
**User Story:** As a project manager, I want to update an active comment.
**AC:** Update comment text if allowed by task state.
**Given:** A comment exists and needs modification.
**When:** A request is sent with method **PATCH** to `/api/v1/tasks/comments/{commentId}` with an `UpdateCommentRequest` body.
**Then:**   If the parent task is still `InProgress`, it saves the change and returns status **200 OK`; otherwise, it returns a `400 Bad Request`.

**US-020**
**User Story:** As a project manager, I want to remove a comment.
**AC:** Soft-delete a comment if allowed by task state.
**Given:** A comment exists and needs deletion.
**When:** A request is sent with method **DELETE** to `/api/v1/tasks/comments/{commentId}`.
**Then:**   If the parent task is `InProgress`, it performs a soft-delete returning status `200 OK`; otherwise, it blocks the removal returning status `400 Bad Request`.

---
