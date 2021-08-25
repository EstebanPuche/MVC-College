using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Intructors
        {
            get; set;
        }
        public IEnumerable<Course> Courses
        {
            get; set;
        }
        public IEnumerable<Enrollment> Enrollments
        {
            get; set;
        }
    }
}
