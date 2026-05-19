### **US-01: Board Management**

* **Story:** As a project manager / team member, I want to create, view, update, and toggle the status of boards inside the app, so that I can organize different projects independently.
* **Acceptance Criteria:**
* User can do CRUD Actions in boards. The system must allow creating, reading, updating, and disabling boards.
* The Title parameter is strictly required and cannot be empty or null.
* Disabling a board must not delete its data; it must perform a soft-delete by setting an isActive flag to false.

#### **Testing for US-01**

* **Create Board:** Since a user wants a board then the app should ask parameters as title and description and return HTTP 201 Created with the new board object upon success.
* **Update Board:** Since has a board then app should allow update title and description returning HTTP 200 OK with the updated data, or HTTP 404 Not Found if the board ID does not exist.
* **Enable/Disable Board:** Since has the status enabled/disable the app should enable or disable the status hiding or showing the task by filtering out disabled boards from the main dashboard views.

#### **Inputs / Outputs for US-01**

* **create board**
* **input:** title, description
* **output:** new object board (status: 201)

* **update board**
* **input:** id, title, description
* **output:** board object (status: 200)

---
### **US-02: Task Management**

* **Story:** As a team member, I want to manage tasks inside a selected board, so that I can keep track of the specific work items required for that project.
* **Acceptance Criteria:**
* User can do CRUD Actions in tasks.
* A task cannot exist without a parent board; a valid boardId must be provided during creation.
* Every new task must automatically start with a default status of "Open".
#### **Testing for US-02**

* **Create Task Success:** Verify that sending a valid title, description, and boardId returns HTTP 201 Created and initializes the task status as "Open".
* **Update Task Success:** Verify that updating a task's fields returns HTTP 200 OK and persists the changes.
* **Create Task Orphan Error:** Verify that trying to create a task with a non-existent boardId returns HTTP 400 Bad Request.

#### **Inputs / Outputs for US-02**

* **create task**
* **input:** title, description, boardId, default status: open
* **output:** new object task (status: 201)


* **update task**
* **input:** id, title, description
* **output:** task object (status: 200)

---
### **US-03: Task Workflow and Status Updates**

* **Story:** As a developer / assignee, I want to move tickets from open to in-progress and before in-progress to done, so that the team knows the real-time status of the work.
* **Acceptance Criteria:**
* User can move tasks from open to in-progress.
* User can move tasks from in-progress to done.
* The workflow must be linear and irreversible: transitions like Open straight to Done, or In-Progress back to Open, must be blocked by the business logic.

#### **Testing for US-03**

* **Task status update I:** Since user can follow a flow with task, then task can change status from "open" to "in-progress" returning HTTP 200 OK.
* **Task status update III:** Since user can follow a flow with task, then task can change status from "in-progress" to "done" returning HTTP 200 OK.
* **Invalid Transition Error:** Verify that trying to move a task from "Open" directly to "Done" returns HTTP 400 Bad Request.

#### **Inputs / Outputs for US-03**

* **update task status**
* **input:** id, status
* **output:** status 200: ok


* **if one task is moved to in-progress**
* **input:** id, status
* **output:** status 200 (ok)

---
### **US-04: Task Collaboration (Comments in-progress)**

* **Story:** As a collaborator, I want to add, view, update, and remove comments only where task is in status: in-progress, so that I can discuss work details while the task is actively being worked on.
* **Acceptance Criteria:**
* User can add comments where ticket status is in-progress.
* User should see all comments that was created.
* User can Remove or add and update comments.
* Any mutation operation—POST, PUT/PATCH, DELETE—on comments must be completely blocked if the task status is "Open" or "Done", throwing an HTTP 400 Bad Request error.
* 
#### **Testing for US-04**

* **Task status update II:** Since user can follow a flow with task, then user can update the task adding comments where status is "in-progress" returning HTTP 200 OK.
* **Get Comments:** Get all comments where task is in-progress returning HTTP 200 OK with the list of comments.
* **Comment Guard Block:** Verify that attempting to add or modify a comment when the task is "Open" or "Done" triggers an HTTP 400 Bad Request.

#### **Inputs / Outputs for US-04**

* **Comments in task where status is in-progress**
* **input:** taskId, comment
* **output:** status 200 (ok) or 201 Created


* **Get all comments where task is in-progress**
* **input:** id (Task ID)
* **output:** status 200 (ok)


* **Remove comment where task is in-progress**
* **input:** id (Comment ID)
* **output:** status 200 (ok)


* **Update comment where task is in-progress**
* **input:** id (Comment ID), comment
* **output:** status 200 (ok)


* **Get all comments in Board where ticket is in-progress**
* **input:** id (Board ID)
* **output:** status 200 (ok)