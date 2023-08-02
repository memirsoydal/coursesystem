namespace CourseSystem.Models;

public class Grade
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<CourseGrade>? CourseGrades { get; set; }
}