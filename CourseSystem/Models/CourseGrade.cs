using System.ComponentModel.DataAnnotations;
namespace CourseSystem.Models;

public class CourseGrade
{
    [Key]
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid GradeId { get; set; }
    public string? CourseGradeName { get; set; }
    public Course? Course { get; set; }
    public Grade? Grade { get; set; }

    public ICollection<TeacherCourse>? TeacherCourses { get; set; }
    
}