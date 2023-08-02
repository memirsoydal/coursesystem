using System.ComponentModel.DataAnnotations;

namespace CourseSystem.Models
{
    public class Content
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SlideUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? HomeworkUrl { get; set; }
        public bool IsHomework { get; set; }
        public List<CourseContent>? CourseContents { get; set; }

    }
}
