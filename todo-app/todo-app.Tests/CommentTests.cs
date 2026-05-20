using Xunit;
using todo_app.Services;

namespace todo_app.Tests
{
    public class CommentTests
    {
        [Fact]
        public void AddComment_TaskInProgress_ShouldSucceed()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");

            var comment = taskService.AddComment(task.Id, "Working on it");

            Assert.NotNull(comment);
            Assert.NotEqual(Guid.Empty, comment.Id);
            Assert.Equal(task.Id, comment.TaskId);
            Assert.Equal("Working on it", comment.CommentText);
            Assert.True(comment.IsActive);
            Assert.True((DateTime.UtcNow - comment.DatePost).TotalSeconds < 5);
        }

        [Fact]
        public void AddComment_TaskOpen_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            Assert.Throws<InvalidOperationException>(() => taskService.AddComment(task.Id, "Cannot comment"));
        }

        [Fact]
        public void AddComment_TaskDone_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            taskService.UpdateTaskStatus(task.Id, "Done");

            Assert.Throws<InvalidOperationException>(() => taskService.AddComment(task.Id, "Cannot comment"));
        }

        [Fact]
        public void UpdateComment_TaskInProgress_ShouldSucceed()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            var comment = taskService.AddComment(task.Id, "Original comment");

            taskService.UpdateComment(comment.Id, "Updated comment");

            var comments = taskService.GetCommentsByTask(task.Id);
            var updated = comments.First(c => c.Id == comment.Id);
            Assert.Equal("Updated comment", updated.CommentText);
        }

        [Fact]
        public void UpdateComment_TaskDone_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            var comment = taskService.AddComment(task.Id, "Original");

            taskService.UpdateTaskStatus(task.Id, "Done");

            Assert.Throws<InvalidOperationException>(() => taskService.UpdateComment(comment.Id, "Updated"));
        }

        [Fact]
        public void DeleteComment_TaskInProgress_ShouldSucceedAndSoftDelete()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            var comment = taskService.AddComment(task.Id, "To delete");

            taskService.DeleteComment(comment.Id);

            var comments = taskService.GetCommentsByTask(task.Id);
            Assert.Empty(comments);
        }

        [Fact]
        public void DeleteComment_TaskOpen_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            var comment = taskService.AddComment(task.Id, "To delete");

            task.Status = "Open";

            Assert.Throws<InvalidOperationException>(() => taskService.DeleteComment(comment.Id));
        }

        [Fact]
        public void GetCommentsByBoard_ShouldOnlyReturnActiveCommentsOfInProgressTasks()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task1 = taskService.CreateTask(board.Id, "Task 1", "Desc");
            var task2 = taskService.CreateTask(board.Id, "Task 2", "Desc");

            taskService.UpdateTaskStatus(task1.Id, "InProgress");
            taskService.UpdateTaskStatus(task2.Id, "InProgress");

            var comment1 = taskService.AddComment(task1.Id, "Comment on task 1");
            var comment2 = taskService.AddComment(task2.Id, "Comment on task 2");

            taskService.UpdateTaskStatus(task2.Id, "Done");

            var boardComments = taskService.GetCommentsByBoard(board.Id);

            Assert.Single(boardComments);
            Assert.Contains(boardComments, c => c.Id == comment1.Id);
            Assert.DoesNotContain(boardComments, c => c.Id == comment2.Id);
        }
    }
}
