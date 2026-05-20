using Xunit;
using todo_app.Services;

namespace todo_app.Tests
{
    public class BoardServiceTests
    {
        [Fact]
        public void Create_ValidBoard_ShouldSucceed()
        {
            var service = new BoardService();
            var board = service.Create("Sprint 1", "Tasks for sprint 1");

            Assert.NotNull(board);
            Assert.NotEqual(Guid.Empty, board.Id);
            Assert.Equal("Sprint 1", board.Title);
            Assert.Equal("Tasks for sprint 1", board.Description);
            Assert.True(board.IsActive);
        }

        [Fact]
        public void GetAll_ShouldOnlyReturnActiveBoardsByDefault()
        {
            var service = new BoardService();
            var board1 = service.Create("Board 1", "Desc");
            var board2 = service.Create("Board 2", "Desc");

            service.SetStatus(board2.Id, false);

            var activeBoards = service.GetAll(includeInactive: false);

            Assert.Single(activeBoards);
            Assert.Contains(activeBoards, b => b.Id == board1.Id);
            Assert.DoesNotContain(activeBoards, b => b.Id == board2.Id);
        }

        [Fact]
        public void GetAll_WithIncludeInactive_ShouldReturnAllBoards()
        {
            var service = new BoardService();
            var board1 = service.Create("Board 1", "Desc");
            var board2 = service.Create("Board 2", "Desc");

            service.SetStatus(board2.Id, false);

            var allBoards = service.GetAll(includeInactive: true);

            Assert.Equal(2, allBoards.Count());
        }

        [Fact]
        public void GetById_InactiveBoard_ShouldThrowKeyNotFoundException()
        {
            var service = new BoardService();
            var board = service.Create("Board 1", "Desc");
            service.SetStatus(board.Id, false);

            Assert.Throws<KeyNotFoundException>(() => service.GetById(board.Id));
        }

        [Fact]
        public void Update_ActiveBoard_ShouldSucceed()
        {
            var service = new BoardService();
            var board = service.Create("Board Old", "Desc Old");

            var updated = service.Update(board.Id, "Board New", "Desc New");

            Assert.Equal("Board New", updated.Title);
            Assert.Equal("Desc New", updated.Description);
        }

        [Fact]
        public void Update_InactiveBoard_ShouldThrowKeyNotFoundException()
        {
            var service = new BoardService();
            var board = service.Create("Board 1", "Desc");
            service.SetStatus(board.Id, false);

            Assert.Throws<KeyNotFoundException>(() => service.Update(board.Id, "New Title", "New Desc"));
        }
    }
}
