### Boards

**US-001**
**User Story:** As a project manager, I want to create a new board via API
    **AC: Create Board successfully**
    **Given:** User wants to create a board
    **When:** sent a request to method POST to "/api/v1/boards" with name of the board
    **Then:** the response should be return a object with status 201 and the Json object must be a "board" schema.
**US-002**  
    **User Story:** As a project manager, I want the API to validate board creation.
    **AC: Prevent create a board without title**
    **Given:** User wants to create a board
    **When:** sent a request to method POST to "/api/v1/boards" with empty title or invalid body
    **Then:** the response should return error with description "Validation or request body error" with status 400.
**US-003**
    **User Story:** As a project manager, I want to retrieve all existing boards via API
    **AC: Retrieve all Boards successfully**
    **Given:** User wants to get a board for reading
    **When:** sent a request to method GET to "/api/v1/boards"
    **Then:** the response should be return a list of object with status 200 therefore Json must be "board" schema.
**US-004**
    **User Story:** As a project manager, I want to update a board's details via API
    **AC: Update Board successfully**
    **Given:** User wants to update a board
    **When:** sent a request to method PATCH to "/api/v1/boards/{id}" with updated title or description
    **Then:** the response should return the updated board object with status 200 and must be a "board" schema.
**US-005**
     **User Story:** As a project manager, I want to receive a clear error when updating a invalid board.
    **AC: Update Board Not Found**
    **Given:** User wants to update a board
    **When:** sent a request to method PATCH to "/api/v1/boards/{id}" with a inexistent board ID
    **Then:** the response should return a status 404 Not Found.
    
**US-006**
     **User Story:** As a project manager, I want to disable a board.
    **AC: Disable Board successfully (Soft-delete)**
    **Given:** User wants to disable a board status
    **When:** sent a request to method PATCH or DELETE to "/api/v1/boards/{id}/disable"
    **Then:** the response should return status 200 and the board data must persist with the isActive flag set to false.
### Tasks

**US-007**  
    **User Story:** As a project manager, I want to add a task to a specific board via API.
    **AC: Create Task successfully with a valid parent board**
    **Given:** User wants to create a task inside a selected board
    **When:** sent a request to method POST to "/api/v1/boards/{boardId}/tasks" with valid task data
    **Then:** the response should return a status 201 Created, link the task to the parent board, and initialize the status automatically as "Open".
**US-008**
    **User Story:** As a project manager, I want to block task creation on missing boards
    **AC: Prevent orphan task creation**
    **Given:** User attempts to create a task
    **When:** sent a request to method POST to "/api/v1/boards/{invalidBoardId}/tasks" with a inexistent board ID
    **Then:** the response should reject the operation and return an error with status 400 Bad Request.
**US-009**
    **User Story:** As a project manager, I want to edit a task's text fields via API.
    **AC: Update Task text fields successfully**
    **Given:** User wants to update a task text fields
    **When:** sent a request to method PUT to "/api/v1/tasks/{id}" with new title or description
    **Then:** the response should persist the changes and return status 200 OK with the updated task data.
**US-010**
    **User Story:** As a project manager, I want to transition a task from Open to In-Progress.
    **AC: Transition status from Open to In-Progress**
    **Given:** A task currently has an "Open" status
    **When:** sent a request to method PATCH to "/api/v1/tasks/{id}/status" with status value "In-Progress"
    **Then:** the response should accept the transition and return status 200 OK.
**US-011**
    **User Story:** As a project manager, I want to transition a task from In-Progress to Done.
    **AC: Transition status from In-Progress to Done**
    **Given:** A task currently has an "In-Progress" status
    **When:** sent a request to method PATCH to "/api/v1/tasks/{id}/status" with status value "Done"
    **Then:** the response should accept the transition, save metadata, and return status 200 OK.
**US-012**
    **User Story:** As a project manager, I want the API to enforce a strict sequential workflow status so that the team cannot bypass required phases or make illegal state changes.
    **AC: Block invalid workflow transitions**
    **Given:** A task currently has an "Open" status
    **When:** sent a request to method PATCH to "/api/v1/tasks/{id}/status" trying to move straight to "Done" (or from In-Progress back to Open)
    **Then:** the business logic should block the operation and return an error with status 400 Bad Request.

### Comments

**US-013**
    **User Story:** As a project manager, I want to add comments to tasks that are In-Progress.
    **AC: Add a comment to an In-Progress task successfully**
    **Given:** A task has a status of "In-Progress"
    **When:** sent a request to method POST to "/api/v1/tasks/{taskId}/comments" with comment content
    **Then:** the response should save the comment and return status 201 Created complying with the comment schema.
**US-014**
    **User Story:** As a project manager, I want to block comment mutations on inactive tasks.
    **AC: Block comment mutations on inactive tasks**
    **Given:** A task has a status of either "Open" or "Done"
    **When:** sent a mutation request (POST, PUT, or DELETE) to the comments endpoint
    **Then:** the response must completely block the execution and return an status error 400 (Bad Request).
**US-015**    
    **User Story:** As a project manager, I want to retrieve all comments from an active task
    **AC: Retrieve all comments of an active task**
    **Given:** Comments exist inside an "In-Progress" task
    **When:** sent a request to method GET to "/api/v1/tasks/{taskId}/comments"
    **Then:** the response should return a list of comment objects with status 200 OK.
**US-016**    
    **User Story:** As a project manager, I want to delete a comment from an active task
    **AC: Remove comment successfully on an active task**
    **Given:** A comment exists and its parent task status is currently "In-Progress"
    **When:** sent a request to method DELETE to "/api/v1/comments/{commentId}"
    **Then:** the system should delete the comment and return status 200 OK.
**US-017**    
    **User Story:** As a project manager, I want to update an existing comment on an active task
    **AC: Update comment successfully on an active task**
    **Given:** A comment exists and its parent task status is currently "In-Progress"
    **When:** sent a request to method PUT to "/api/v1/comments/{commentId}" with new content
    **Then:** the system should apply the modification and return status 200 OK with the updated comment object.