using System.ComponentModel.DataAnnotations;

namespace CourseSystem.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CourseContent>? CourseContents { get; set; }
        public List<CourseGrade>? CourseGrades { get; set; }
    }
}