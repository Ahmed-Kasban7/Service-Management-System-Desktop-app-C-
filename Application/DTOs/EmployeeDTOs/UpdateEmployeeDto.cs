using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs
{
    public class UpdateEmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public byte Sex { get; set; }
        public string ?  Address { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}
