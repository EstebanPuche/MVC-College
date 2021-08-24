using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class Course
    {
        [Display(Name = "Número")]
        public int CourseId
        {
            get; set;
        }

        [StringLength(50, MinimumLength = 3)]
        public string Title
        {
            get; set;
        }

        //! El atributo "Range" crea un rango comprendido en unos prámetros numéricos que se le pasan
        [Range(0, 5)]
        public int Credits
        {
            get; set;
        }

        /**
         * "Course" tiene una propiedad de clave externa "DepartamentId" que señala a la entidad "Departament"
         * A un curso se asigna a un departamento, por lo que hay una clave externa DepartmentId"
         */
        public int DepartmentId
        {
            get; set;
        }
        // Y tiene una propiedad de navegación "Departament"
        /** Y una propiedad de navegación "Department" por las razones mencionadas anteriormente */
        public Department Department
        {
            get; set;
        }


        /**
         * La propiedad "Enrollments" es una propiedad de navegación. Una entidad "Course" puede estar 
         * relacionada con cualquier número dde entidades "Enrollment".
         * Un curso puede tener cualquier número de alumnos inscritos en él, por lo que la propiedad de 
         * navegación "Enrollment" es una colección
         */
        public ICollection<Enrollment> Enrollments
        {
            get; set;
        }

        /**
         * Un curso puede ser impartido por varios instructores, por lo que la propiedad de navegación
         * "CourseAssignments" es una colección.
         */
        public ICollection<CourseAssignment> CourseAssignments
        {
            get; set; 
        }
    }
}
