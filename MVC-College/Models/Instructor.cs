using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class Instructor
    {
        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "El apellido es un campo obligatorio.")]
        [Display(Name = "Apellido")]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "El apellido debe empezar por mayúscula y no tener números.")]
        public string LastName
        {
            get; set;
        }

        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        [Display(Name = "Nombre")]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "El nombre debe empezar por mayúscula y no tener números.")]
        public string FirstMidName
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de contratación")]
        public DateTime HireDate
        {
            get; set;
        }

        [Display(Name = "Nombre completo")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        /**
         * Propiedad de navegación "CourseAssignments" un instructor puede impartir cualquier número de
         * cursos, por lo que "CourseAssignments" se define como una colección.
         */
        public ICollection<CourseAssignment> CourseAssignments
        {
            get; set;
        }
        /**
         * Las reglas de negocion "MVC_college" estblecen que un instructor solo puede tener una oficina
         * a lo sumo, por lo que la propiedad "OfficeAssignment" contiene una única entidad "OfficeAssignment"
         */
        public OfficeAssignment OfficeAssignment
        {
            get; set;
        }

    }
}
