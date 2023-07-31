using System.ComponentModel.DataAnnotations;

namespace CourseSystem.Models
{
    public class Icerik
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Slide_Url { get; set; }
        public string? Video_Url { get; set; }
        public string? Pdf_Url { get; set; }
        public bool Is_Homework { get; set; }
        public List<DersIcerik>? CourseContents { get; set; }

    }
}
