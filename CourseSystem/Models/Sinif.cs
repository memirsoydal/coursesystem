namespace CourseSystem.Models;

public class Sinif
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<DersSinif>? CourseSinifs { get; set; }
}