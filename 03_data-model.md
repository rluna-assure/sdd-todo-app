Models: The next models are needed for the project, create models in language selected.

- tasks
	- id: int
	- title: string 
	- description: string
	- is-active: Boolean
	- board-id: board.id
	- status-enum (OPEN,IN-PROGRESS,DONE)
	- open-date-date: date
	- close-date-date: date

- boards 
	- id: int
	- title: string
	- description: string
	- is-active: Boolean

- comments
	- id: int
	- comment: string
	- date-post: date
	
- task-comments
	- id: int
	- comments.id: int(foreign-key)
	- tasks.id: int(foreign-key)
	