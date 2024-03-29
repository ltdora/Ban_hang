using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class Employees
    {
        [Key]
        public int EmployeeId { set; get; }
        public string EmployeeName { set; get; }
        public string EmployeePassword { set; get; }
    }
}
