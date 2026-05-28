Models: The next models are needed for the project, create models in language selected.

- tasks
	- id: Guid
	- title: string 
	- description: string
	- is-active: Boolean
	- board-id: board.id
	- status-enum (OPEN,IN-PROGRESS,DONE)
	- open-date: date
	- close-date: date

- boards 
	- id: Guid
	- title: string
	- description: string
	- is-active: Boolean

- comments
	- id: Guid
	- comment: string
	- date-post: date
	
- task-comments
	- id: Guid
	- comments.id: (foreign-key)
	- tasks.id: (foreign-key)
	