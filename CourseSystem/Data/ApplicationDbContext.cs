using CourseSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<CourseContent> CourseContents { get; set;}
        public DbSet<CourseGrade> CourseGrades { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CourseContent>()
                .HasKey(cl => new { cl.CourseId, cl.ContentId });
            modelBuilder.Entity<TeacherCourse>()
                .HasKey(cl => new { cl.TeacherId, cl.CourseGradeId });
            
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            //Seeding a  'Administrator' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole {Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN".ToUpper() });


            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<ApplicationUser>();


            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    FirstName = "ADMIN",
                    LastName = "ADMIN",
                    FullName = "ADMIN ADMIN",
                    UserName = "myuser",
                    NormalizedUserName = "MYUSER",
                    PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210", 
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
        }
    }
}
