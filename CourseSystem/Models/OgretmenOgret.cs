using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSystem.Models;

public class OgretmenOgret
{
    // TODO OgretmenOgret modelinde iki tane TeacherId var. Neden? Create islemini gelince test et.
    [Key]
    [Column(Order = 1)]
    public string TeacherId { get; set; }
    [Key]
    [Column(Order = 2)]
    public Guid CourseSinifId { get; set; }
    public Kullanici? Teacher { get; set; }
    public DersSinif? CourseSinif { get; set; }
}