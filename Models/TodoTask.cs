using System.ComponentModel.DataAnnotations;

namespace Todo_list.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty; // Thay 'required string' bằng 'string' và gán giá trị mặc định

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hoàn thành")]
        public bool IsCompleted { get; set; } = false;

        [Display(Name = "Hạn chót")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}