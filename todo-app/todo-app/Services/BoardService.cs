using System.Data;
using todo_app.Models;

namespace todo_app.Services
{
    public class BoardService : IBoardService
    {
        private readonly List<Board> _boards = new List<Board>();

        public IEnumerable<Board> GetAll(bool includeInactive = false)
        {
            return _boards.Where(b => includeInactive || b.IsActive).ToList();
        }

        public Board GetById(Guid id)
        {
            var board = _boards.FirstOrDefault(b => b.Id == id);
            if (board == null || !board.IsActive)
            {
                throw new KeyNotFoundException("Board not found or inactive");
            }
            return board;
        }

        public Board Create(string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required", nameof(title));
            }

            var board = new Board
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description ?? "",
                IsActive = true
            };

            _boards.Add(board);
            return board;
        }

        public Board Update(Guid id, string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required", nameof(title));
            }

            var board = _boards.FirstOrDefault(b => b.Id == id);
            if (board == null || !board.IsActive)
            {
                throw new KeyNotFoundException("Board not found or inactive");
            }

            board.Title = title;
            board.Description = description ?? "";
            return board;
        }

        public bool SetStatus(Guid id, bool isActive)
        {
            var board = _boards.FirstOrDefault(b => b.Id == id);
            if (board == null)
            {
                return false;
            }

            board.IsActive = isActive;
            return true;
        }
    }
}
