  ### **Testing**
 
- Create Board: Since a user wants a board then then app should ask parameters as title and description.
- Update Board: Since has a board then app should allow update title and description.
- Enable/Disable Board: Since has the status enabled/disable the app should enable or disable the status hiding or showing the task.
- Create Board: Since a user wants a board then then app should ask parameters as title and description.
- Update Board: Since has a board then app should allow to update title and description.
- Enable/Disable Board: Since tasks has the status enabled the app should enable or disable the status hiding or showing the task.
- Task status update I: Since user can follow a flow with task, then task can change status from "open" to "in-progress".
- Task status update II: Since user can follow a flow with task, then user can update the task adding comments where status is "in-progress".
- Task status update III: Since user can follow a flow with task, then task can change status from "in-progress" to "done".

 ### **Stories**
	- As user I want to do a CRUD boards inside the app.
		- AC: user can do CRUD Actions in boards.
	- As user I want to do a CRUD tasks inside board selected.
		- AC: user can do CRUD Actions in tasks.
	- As user I want to move tickets from open to in-progress and before in-progress to close.
		- AC: 
		    user can move tasks from open to in-progress
		    user can move tasks from in-progress to close
		    user can add comments where tickets is in-progress.
	Allow to Add comments where task is in status: in-progress
		User can ad add comments where ticket status is in-progress
		User should see all comments that was created
		User can Remove or add 
	 Get all comments where task is in-progress
	
 ### **Input/Outputs**
	- create board
	    - input: title, description
	    - output: new object task (status: 201)
	- update board
	    - input: id, title
	    - output: board object (status: 200)
	- create task
	    - input: title, description, default status: open
	    - output: new object task (status: 201)
	- update task
	    - input: id, title
	    - output: task object: (status: 200)
	- update task status
	    - input: id, status
	    - output: status 200: ok
	- if one task is moved to in-progress
	    - input: id, status
	    - output: status 200 (ok)
	- Comments in task where status is in-progress
	    - input: id, comment
	    - output: status 200 (ok)
	- Get all comments where task is in-progress
	    - input: id
	    - output: status 200 (ok)
	- Remove comment where task is in-progress
	    - input: id
	    - output: status 200 (ok)
	- Update comment where task is in-progress
	    - input: id, comment
	    - output: status 200 (ok)
	- Get all comments in Board where ticket is in-progress
	    - input: id
	    - output: status 200 (ok)