using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CourseSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    [ForeignKey("Grade")]
    public Guid? GradeId { get; set; }
    public ICollection<TeacherCourse>? TeacherCourses { get; set; }
}