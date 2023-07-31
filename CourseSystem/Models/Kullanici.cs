using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CourseSystem.Models;

public class Kullanici : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    [ForeignKey("Sinif")]
    public Guid? SinifId { get; set; }
    public ICollection<OgretmenOgret>? CoursesTaught { get; set; }
}