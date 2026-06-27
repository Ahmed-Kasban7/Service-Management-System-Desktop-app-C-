using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs;

public class EmployeeSummaryDto
{
    public int EmployeeId { get; set; }
    public string EmployeeNumber { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string DepartmentName { get; set; }
    public string RoleName { get; set; }
}
