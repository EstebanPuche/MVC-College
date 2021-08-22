using System;
using System.Collections.Generic;
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
        public string LastName
        {
            get; set;
        }
        public string FisrtMidName
        {
            get; set;
        }
        public DateTime EnrollmentDate
        {
            get; set;
        }
        /**
         * La propiedad "Enrollment" es una proipiedad de navegación, contiene otra entidad realacionada con esta entidad.
         */
        public ICollection<Enrollment> Enrollments
        {
            get; set;
        }
    }
}
