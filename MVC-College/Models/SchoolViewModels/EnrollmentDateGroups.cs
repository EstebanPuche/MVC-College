using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_College.Models.SchoolViewModels
{
    public class EnrollmentDateGroups
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate
        {
            get; set;
        }
        public int StudentCount
        {
            get; set;
        }
    }
}
