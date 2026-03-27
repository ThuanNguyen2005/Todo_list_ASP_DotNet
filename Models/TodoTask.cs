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
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hoàn thành")]
        public bool trangThai { get; set; } = false;

        [Display(Name = "Danh mục")]
        public string? danhMuc { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Hạn chót")]
        public DateTime? Deadline { get; set; }
    }
}