using CourseSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Ders> Courses { get; set; }
        public DbSet<Icerik> Contents { get; set; }
        public DbSet<Sinif> Sinifs { get; set; }
        public DbSet<DersIcerik> CourseContents { get; set;}
        public DbSet<DersSinif> CourseSinif { get; set; }
        public DbSet<OgretmenOgret> TeacherTeaches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<DersIcerik>()
                .HasKey(cl => new { cl.CourseId, cl.ContentId });
            modelBuilder.Entity<OgretmenOgret>()
                .HasKey(cl => new { cl.TeacherId, cl.CourseSinifId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
