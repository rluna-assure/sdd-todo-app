using System.Data;
using todo_app.Models;

namespace todo_app.Services
{
    public class TaskService : ITaskService
    {
        private readonly IBoardService _boardService;
        private readonly List<todo_app.Models.Task> _tasks = new List<todo_app.Models.Task>();
        private readonly List<Comment> _comments = new List<Comment>();

        public TaskService(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public todo_app.Models.Task CreateTask(Guid boardId, string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required", nameof(title));
            }

            var board = _boardService.GetById(boardId);

            var task = new todo_app.Models.Task
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description ?? "",
                BoardId = boardId,
                Status = "Open",
                IsActive = true,
                OpenDate = DateTime.UtcNow,
                CloseDate = null
            };

            _tasks.Add(task);
            return task;
        }

        public todo_app.Models.Task GetTaskById(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || !task.IsActive)
            {
                throw new KeyNotFoundException("Task not found or inactive");
            }

            var board = _boardService.GetById(task.BoardId);
            return task;
        }

        public todo_app.Models.Task UpdateTask(Guid id, string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required", nameof(title));
            }

            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || !task.IsActive)
            {
                throw new KeyNotFoundException("Task not found or inactive");
            }

            var board = _boardService.GetById(task.BoardId);

            task.Title = title;
            task.Description = description ?? "";
            return task;
        }

        public IEnumerable<todo_app.Models.Task> GetTasksByBoard(Guid boardId, bool includeInactive = false)
        {
            var board = _boardService.GetById(boardId);

            return _tasks.Where(t => t.BoardId == boardId && (includeInactive || t.IsActive)).ToList();
        }

        public void UpdateTaskStatus(Guid id, string status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || !task.IsActive)
            {
                throw new KeyNotFoundException("Task not found or inactive");
            }

            var board = _boardService.GetById(task.BoardId);

            if (task.Status == "Open" && status == "InProgress")
            {
                task.Status = "InProgress";
            }
            else if (task.Status == "InProgress" && status == "Done")
            {
                task.Status = "Done";
                task.CloseDate = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException("Invalid status transition");
            }
        }

        public Comment AddComment(Guid taskId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                throw new ArgumentException("Comment text is required", nameof(commentText));
            }

            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            if (!task.IsActive || task.Status != "InProgress")
            {
                throw new InvalidOperationException("Comments can only be added to InProgress active tasks");
            }

            var board = _boardService.GetById(task.BoardId);

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                CommentText = commentText,
                DatePost = DateTime.UtcNow,
                IsActive = true
            };

            _comments.Add(comment);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null || !task.IsActive)
            {
                throw new KeyNotFoundException("Task not found or inactive");
            }

            var board = _boardService.GetById(task.BoardId);

            return _comments.Where(c => c.TaskId == taskId && c.IsActive).OrderBy(c => c.DatePost).ToList();
        }

        public void UpdateComment(Guid commentId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                throw new ArgumentException("Comment text is required", nameof(commentText));
            }

            var comment = _comments.FirstOrDefault(c => c.Id == commentId && c.IsActive);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found or inactive");
            }

            var task = _tasks.FirstOrDefault(t => t.Id == comment.TaskId);
            if (task == null || !task.IsActive || task.Status != "InProgress")
            {
                throw new InvalidOperationException("Comments can only be modified on InProgress active tasks");
            }

            var board = _boardService.GetById(task.BoardId);

            comment.CommentText = commentText;
        }

        public void DeleteComment(Guid commentId)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId && c.IsActive);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found or inactive");
            }

            var task = _tasks.FirstOrDefault(t => t.Id == comment.TaskId);
            if (task == null || !task.IsActive || task.Status != "InProgress")
            {
                throw new InvalidOperationException("Comments can only be deleted from InProgress active tasks");
            }

            var board = _boardService.GetById(task.BoardId);

            comment.IsActive = false;
        }

        public IEnumerable<Comment> GetCommentsByBoard(Guid boardId)
        {
            var board = _boardService.GetById(boardId);

            var activeInProgressTaskIds = _tasks
                .Where(t => t.BoardId == boardId && t.IsActive && t.Status == "InProgress")
                .Select(t => t.Id)
                .ToHashSet();

            return _comments
                .Where(c => activeInProgressTaskIds.Contains(c.TaskId) && c.IsActive)
                .ToList();
        }
    }
}
