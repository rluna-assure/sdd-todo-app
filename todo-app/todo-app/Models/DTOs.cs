using System.ComponentModel.DataAnnotations;

namespace todo_app.Models
{
    public class CreateBoardRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateBoardRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class SetBoardStatusRequest
    {
        [Required]
        public bool IsActive { get; set; }
    }

    public class CreateTaskRequest
    {
        [Required]
        public Guid BoardId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public class UpdateTaskRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public class UpdateTaskStatusRequest
    {
        [Required]
        [RegularExpression("^(Open|InProgress|Done)$")]
        public string Status { get; set; }
    }

    public class AddCommentRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Comment { get; set; }
    }

    public class UpdateCommentRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Comment { get; set; }
    }
}
