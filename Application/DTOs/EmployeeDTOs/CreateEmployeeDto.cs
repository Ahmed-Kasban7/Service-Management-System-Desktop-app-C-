using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs;


public class CreateEmployeeDto
{
    public string Name { get; set; }
    public int? Age { get; set; }
    public byte Sex { get; set; }
    public string ? Address { get; set; }
    public DateTime HireDate { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
    public int CompensationType { get; set; }

    public decimal? BaseSalary { get; set; }
    public decimal? CommissionPercent { get; set; }
    public List<string> Phones { get; set; } = new();
    public List<string> Attachments { get; set; } = new();
}