using System.ComponentModel.DataAnnotations;
namespace CourseSystem.Models;

public class DersSinif
{
    [Key]
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid SinifId { get; set; }
    public Ders? Course { get; set; }
    public Sinif? Sinif { get; set; }

    public ICollection<OgretmenOgret>? Teachers { get; set; }
    
}