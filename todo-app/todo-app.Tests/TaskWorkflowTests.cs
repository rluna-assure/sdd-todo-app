using Xunit;
using todo_app.Services;

namespace todo_app.Tests
{
    public class TaskWorkflowTests
    {
        [Fact]
        public void CreateTask_ValidActiveBoard_ShouldSucceed()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Task Desc");

            Assert.NotNull(task);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.Equal("Task 1", task.Title);
            Assert.Equal("Task Desc", task.Description);
            Assert.Equal("Open", task.Status);
            Assert.True(task.IsActive);
            Assert.True((DateTime.UtcNow - task.OpenDate).TotalSeconds < 5);
            Assert.Null(task.CloseDate);
        }

        [Fact]
        public void CreateTask_NonExistentBoard_ShouldThrowKeyNotFoundException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            Assert.Throws<KeyNotFoundException>(() => taskService.CreateTask(Guid.NewGuid(), "Task 1", "Desc"));
        }

        [Fact]
        public void CreateTask_InactiveBoard_ShouldThrowKeyNotFoundException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            boardService.SetStatus(board.Id, false);

            Assert.Throws<KeyNotFoundException>(() => taskService.CreateTask(board.Id, "Task 1", "Desc"));
        }

        [Fact]
        public void UpdateTaskStatus_OpenToInProgress_ShouldSucceed()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");

            var updated = taskService.GetTaskById(task.Id);
            Assert.Equal("InProgress", updated.Status);
        }

        [Fact]
        public void UpdateTaskStatus_InProgressToDone_ShouldSucceedAndSetCloseDate()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            taskService.UpdateTaskStatus(task.Id, "Done");

            var updated = taskService.GetTaskById(task.Id);
            Assert.Equal("Done", updated.Status);
            Assert.NotNull(updated.CloseDate);
            Assert.True((DateTime.UtcNow - updated.CloseDate.Value).TotalSeconds < 5);
        }

        [Fact]
        public void UpdateTaskStatus_OpenToDone_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            Assert.Throws<InvalidOperationException>(() => taskService.UpdateTaskStatus(task.Id, "Done"));
        }

        [Fact]
        public void UpdateTaskStatus_InProgressToOpen_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");

            Assert.Throws<InvalidOperationException>(() => taskService.UpdateTaskStatus(task.Id, "Open"));
        }

        [Fact]
        public void UpdateTaskStatus_DoneToInProgress_ShouldThrowInvalidOperationException()
        {
            var boardService = new BoardService();
            var taskService = new TaskService(boardService);

            var board = boardService.Create("Board 1", "Desc");
            var task = taskService.CreateTask(board.Id, "Task 1", "Desc");

            taskService.UpdateTaskStatus(task.Id, "InProgress");
            taskService.UpdateTaskStatus(task.Id, "Done");

            Assert.Throws<InvalidOperationException>(() => taskService.UpdateTaskStatus(task.Id, "InProgress"));
        }
    }
}
