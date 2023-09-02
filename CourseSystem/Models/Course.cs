using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSystem.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<CourseContent>? CourseContents { get; set; }
        public List<CourseGrade>? CourseGrades { get; set; }
    }
}