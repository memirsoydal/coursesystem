using System.ComponentModel.DataAnnotations;

namespace CourseSystem.Models
{
    public class Ders
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DersIcerik>? CourseContents { get; set; }
        public List<DersSinif>? CourseSinifs { get; set; }
    }
}
