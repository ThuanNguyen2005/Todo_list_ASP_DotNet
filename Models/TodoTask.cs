using System.ComponentModel.DataAnnotations;

namespace Todo_list.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; } // Thêm dòng này để lưu ID của người dùng tạo task
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}