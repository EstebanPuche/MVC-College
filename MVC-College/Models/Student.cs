using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class Student
    {
        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "El apellido es un campo obligatorio.")]
        //! El atributo "StringLength" estable la longuitud máxima de la base de datos y validación del lado cliente.
        [StringLength(30, MinimumLength = 3)]
        //! El atributo "RegularExpresion" aplica restricniones de entrada. Por ejemplo primer caracter mayuscula y el resto alfabéticos
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "El apellido debe empezar por mayúscula y no tener números.")]
        [Display(Name = "Apellido")]
        public string LastName
        {
            get; set;
        }

        [Required(ErrorMessage = "El nombre es un campo obligatorio.")]
        //! El atributo "StringLength" estable la longuitud máxima de la base de datos y validación del lado cliente.
        [StringLength(30, MinimumLength = 3)]
        //! El atributo "RegularExpresion" aplica restricniones de entrada. Por ejemplo primer caracter mayuscula y el resto alfabéticos
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "El nombre debe empezar por mayúscula y no tener números.")]
        [Display(Name = "Nombre")]
        public string FisrtMidName
        {
            get; set;
        }

        //! Atributo "DataType" especifica el tipo de dato
        [DataType(DataType.Date)]
        //! El atributo "DisplayFormat" se utiliza para especificar el formato.
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Inscripción")]
        public DateTime EnrollmentDate
        {
            get; set;
        }

        //! La propiedad calculada "FullName" devuelve el valor que se crea mediante la concvatenación de otras propiedades.
        [Display(Name = "Nombre completo")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FisrtMidName;
            }
        }

        /**
         * La propiedad "Enrollment" es una proipiedad de navegación, contiene otra entidad realacionada
         * con esta entidad.
         */
        public ICollection<Enrollment> Enrollments
        {
            get; set;
        }
    }
}
