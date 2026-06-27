using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.EmployeeDTOs;

public class EmployeeProfileDto
{
    public int Id { get; set; }
    public string EmployeeNumber { get; set; }
    public string Name { get; set; }
    public string ?  Address { get; set; }
    public int ?  Age  { get; set; }
    public byte Sex { get; set; }
    public DateTime HireDate { get; set; }
    public string DepartmentName { get; set; }
    public string RoleName { get; set; }
    public byte CompensationType { get; set; }
    public string CompensationTypeText { get; set; }
    public decimal BaseSalary { get; set; }
    public int CommissionPercent { get; set; }
}
