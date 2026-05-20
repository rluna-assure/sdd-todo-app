namespace todo_app.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string CommentText { get; set; }
        public DateTime DatePost { get; set; }
        public bool IsActive { get; set; }
    }
}
