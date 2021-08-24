using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class OfficeAssignment
    {
        /**
         * Hay una relación de "uno a cero o uno" entre "Instructor" y las entidades "OfficeAssignment".
         * Una asignación de oficina solo existe en relación con el instructor al que se le asigna y, 
         * por lo tanto, su clave principal tambien es su clave externa para la entidad "Instructor".
         * Pero Entity Framework no reconoce automáticamente "IntructorId" como clave principal de esta 
         * entidad por que su nombre no sigue la concención de nomenclatura de "Id". Por tanto, se usa
         * el atributo "Key" para identificarla como la clave.
         */
        [Key]
        public int InstructorId
        {
            get; set;
        }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Localización de la oficina")]
        public string Location
        {
            get; set;
        }

        /**
         * Propiedad de navegación Instructor
         * La entidad Instructor tiene una propiedad de navegación OfficeAssignment que acepta valores NULL
         * (porque es posible que no se asigne una oficina a un instructor), y la entidad OfficeAssignment
         * tiene una propiedad de navegación Instructor que no acepta valores NULL (porque una asignación
         * de oficina no puede existir sin un instructor; InstructorID no acepta valores NULL). Cuando una
         * entidad Instructor tiene una entidad OfficeAssignment relacionada, cada entidad tendrá una 
         * referencia a la otra en su propiedad de navegación.
         */
        public Instructor Instructor
        {
            get; set;
        }
    }
}
