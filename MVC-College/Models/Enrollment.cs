using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentId
        {
            get; set;
        }

        /**
         * Un registro de inscripción es para un solo curso, por lo que hay una propiedad de clave externa
         *  "CourseId"
         */
        public int CourseId
        {
            get; set;
        }

        /**
         * Un registro de inscripción es para un solo estudiante, por lo que hay una propiedad de clave externa
         * "StudentId"
         */
        public int StudentId
        {
            get; set;
        }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade
        {
            get; set;
        }

        /**
         * Propieda de navegación a "Course"
         */
        public Course Course
        {
            get; set;
        }

        /**
         * Propiedad de navegació a "Student"
         */
        public Student Student
        {
            get; set;
        }
    }
}
