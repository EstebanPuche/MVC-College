using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class Department
    {
        public int DepartmentId
        {
            get; set;
        }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string Name
        {
            get; set;
        }

        /**
         * El atributo column cambia la asignacion de nombres de columna
         */
        [DataType(DataType.Currency)]
        [Column(TypeName = "Decimal(18, 2)")]
        [Display(Name = "Presupuesto")]
        public decimal Budget
        {
            get; set; 
        }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha inicio")]
        public DateTime StartDate
        {
            get; set;
        }

        /**
         * Un departamento puede tener o no un administrador "?", y un administrador es siempre un instructor.
         * Por lo tanto, la propiedad "IntructorId" se incluye como la clave externa de la entidad Instructor
         * y se agrega un signo de interrogación después de la designación del tipo int para marcar la 
         * proppiedad como acepta valores NULL.
         */
        public int? InstructorId
        {
            get; set;
        }

        /**
         * La propiedad de navegación se denomina "Administrator" pero contiene una entidad Instructor.
         */
        public Instructor Administrator
        {
            get; set;
        }

        /**
         * Un departamento puede tener varios cursos, por que lo hay una propiedad de navegación "Courses"
         */
        public ICollection<Course> Courses
        {
            get; set;
        }
    }
}
