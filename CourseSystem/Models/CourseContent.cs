using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSystem.Models
{
    public class CourseContent
    {
        [Key]
        [Column(Order = 1)]
        public Guid CourseId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid ContentId { get; set; }
        public Course? Course { get; set; }
        public Content? Content { get; set; }
        public int Sequence { get; set; }
    }
}
