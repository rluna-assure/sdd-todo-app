using Microsoft.AspNetCore.Mvc;
using todo_app.Models;
using todo_app.Services;

namespace todo_app.Controllers
{
    [ApiController]
    [Route("/api/v1/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public ActionResult<todo_app.Models.Task> Create([FromBody] CreateTaskRequest request)
        {
            try
            {
                var task = _taskService.CreateTask(request.BoardId, request.Title, request.Description);
                return Created($"/api/v1/tasks/{task.Id}", task);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return BadRequest();
                }
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                throw;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<todo_app.Models.Task> Get([FromRoute] string id)
        {
            try
            {
                var task = _taskService.GetTaskById(new Guid(id));
                return Ok(task);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpPatch("{id}")]
        public ActionResult<todo_app.Models.Task> Update([FromRoute] string id, [FromBody] UpdateTaskRequest request)
        {
            try
            {
                var task = _taskService.UpdateTask(new Guid(id), request.Title, request.Description);
                return Ok(task);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpGet("board/{boardId}")]
        public ActionResult<List<todo_app.Models.Task>> GetTasksByBoard([FromRoute] string boardId, [FromQuery] bool includeInactive = false)
        {
            try
            {
                var tasks = _taskService.GetTasksByBoard(new Guid(boardId), includeInactive).ToList();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpPost("{id}/status")]
        public ActionResult UpdateStatus([FromRoute] string id, [FromBody] UpdateTaskStatusRequest request)
        {
            try
            {
                _taskService.UpdateTaskStatus(new Guid(id), request.Status);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpPost("{id}/comment")]
        public ActionResult<Comment> AddComment([FromRoute] string id, [FromBody] AddCommentRequest request)
        {
            try
            {
                var comment = _taskService.AddComment(new Guid(id), request.Comment);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpGet("{id}/comments")]
        public ActionResult<List<Comment>> GetComments([FromRoute] string id)
        {
            try
            {
                var comments = _taskService.GetCommentsByTask(new Guid(id)).ToList();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpPatch("comments/{commentId}")]
        public ActionResult UpdateComment([FromRoute] string commentId, [FromBody] UpdateCommentRequest request)
        {
            try
            {
                _taskService.UpdateComment(new Guid(commentId), request.Comment);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpDelete("comments/{commentId}")]
        public ActionResult DeleteComment([FromRoute] string commentId)
        {
            try
            {
                _taskService.DeleteComment(new Guid(commentId));
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound();
                }
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }
    }
}
