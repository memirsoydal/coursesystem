using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSystem.Models;

public class TeacherCourse
{
    [Key]
    [Column(Order = 1)]
    public string TeacherId { get; set; }
    [Key]
    [Column(Order = 2)]
    public Guid CourseGradeId { get; set; }
    public ApplicationUser? Teacher { get; set; }
    public CourseGrade? CourseGrade { get; set; }
}