using todo_app.Models;

namespace todo_app.Services
{
    public interface IBoardService
    {
        IEnumerable<Board> GetAll(bool includeInactive = false);
        Board GetById(Guid id);
        Board Create(string title, string? description);
        Board Update(Guid id, string title, string? description);
        bool SetStatus(Guid id, bool isActive);
    }
}
