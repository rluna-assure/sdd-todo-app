using Microsoft.AspNetCore.Mvc;
using todo_app.Models;
using todo_app.Services;

namespace todo_app.Controllers
{
    [ApiController]
    [Route("/api/v1/boards")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly ITaskService _taskService;

        public BoardController(IBoardService boardService, ITaskService taskService)
        {
            _boardService = boardService;
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<Board>> GetAll([FromQuery] bool includeInactive = false)
        {
            var result = _boardService.GetAll(includeInactive).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Board> Get([FromRoute] string id)
        {
            try
            {
                var board = _boardService.GetById(new Guid(id));
                return Ok(board);
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

        [HttpPost]
        public ActionResult<Board> Create([FromBody] CreateBoardRequest request)
        {
            try
            {
                var board = _boardService.Create(request.Title, request.Description);
                return Created($"/api/v1/boards/{board.Id}", board);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                throw;
            }
        }

        [HttpPatch("{id}")]
        public ActionResult<Board> Update([FromRoute] string id, [FromBody] UpdateBoardRequest request)
        {
            try
            {
                var result = _boardService.Update(new Guid(id), request.Title, request.Description);
                return Ok(result);
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

        [HttpPost("{id}/status")]
        public ActionResult SetStatus([FromRoute] string id, [FromBody] SetBoardStatusRequest request)
        {
            try
            {
                var success = _boardService.SetStatus(new Guid(id), request.IsActive);
                if (!success)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    return BadRequest();
                }
                throw;
            }
        }

        [HttpGet("{boardId}/comments")]
        public ActionResult<List<Comment>> GetBoardComments([FromRoute] string boardId)
        {
            try
            {
                var comments = _taskService.GetCommentsByBoard(new Guid(boardId)).ToList();
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
    }
}
