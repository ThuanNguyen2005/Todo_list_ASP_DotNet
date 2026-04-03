using System.ComponentModel.DataAnnotations;

namespace Todo_list.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;
        public ICollection<TodoTask>? TodoTasks { get; set; }
    }
}