using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models
{
    public class Course
    {
        
        public int CourseId
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public int Credits
        {
            get; set;
        }
        /**
         * La propiedad "Enrollments" es una propiedad de navegación. Una entidad "Course" puede estar 
         * relacionada con cualquier número dde entidades "Enrollment".
         */
        public ICollection<Enrollment> Enrollments
        {
            get; set;
        }
    }
}
