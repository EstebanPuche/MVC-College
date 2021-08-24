using Microsoft.EntityFrameworkCore;
using MVC_College.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Data
{
    public class MVC_CollegeContexto : DbContext
    {
        public MVC_CollegeContexto(DbContextOptions<MVC_CollegeContexto> options) : base(options)
        {

        }
        /**
         * Se crea las tablas de la base de datos.
         */
        public DbSet<Course> Courses
        {
            get; set;
        }
        public DbSet<Enrollment> Enrollments
        {
            get; set;
        }
        public DbSet<Student> Students
        {
            get; set;
        }
        public DbSet<Department> Departments
        {
            get; set;
        }
        public DbSet<Instructor> Instructors
        {
            get; set;
        }
        public DbSet<OfficeAssignment> OfficeAssignments
        {
            get; set;
        }
        public DbSet<CourseAssignment> CourseAssignments
        {
            get; set;
        }


        /**
         * Se invalida el comportamiento predeterminado de que el nombre dde las ttablas sea en plural
         * predeterminando mediante la especificación de nombres de tabla en singular en "DbContext".
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            //! Se configura la clave principal compuesta de la entidad "CourseAssignment"
            modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseId, c.InstructorId });

            // Deshabilitamos la eliminación de la tablas relacionadas en cascada
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
