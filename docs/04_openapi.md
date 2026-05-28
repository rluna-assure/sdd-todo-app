The endpoint should have the next configuration: 

api/v1/:{route}

Boards
GET: api/v1/get/{id}
POST: api/v1/
body:{
	title
}
PATCH: api/v1/{id}
body: {
	title: string,
	description: string
}
POST: api/v1/{v1}/status {  
	isActive: true  
}

tasks
GET: api/v1/get/{id}
POST: api/v1/
body:{
	title
}
PATCH: api/v1/{id}
body: {
	title: string,
	description: string
}
POST: api/v1/{v1}/status {  
	isActive: true  
}

POST: api/v1/{v1}/comment {  
	isActive: true  
}
POST: api/v1/{v1}/comment {  
	isActive: true  
}