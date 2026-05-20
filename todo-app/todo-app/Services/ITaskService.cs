using todo_app.Models;

namespace todo_app.Services
{
    public interface ITaskService
    {
        todo_app.Models.Task CreateTask(Guid boardId, string title, string? description);
        todo_app.Models.Task GetTaskById(Guid id);
        todo_app.Models.Task UpdateTask(Guid id, string title, string? description);
        IEnumerable<todo_app.Models.Task> GetTasksByBoard(Guid boardId, bool includeInactive = false);
        void UpdateTaskStatus(Guid id, string status);
        Comment AddComment(Guid taskId, string commentText);
        IEnumerable<Comment> GetCommentsByTask(Guid taskId);
        void UpdateComment(Guid commentId, string commentText);
        void DeleteComment(Guid commentId);
        IEnumerable<Comment> GetCommentsByBoard(Guid boardId);
    }
}
