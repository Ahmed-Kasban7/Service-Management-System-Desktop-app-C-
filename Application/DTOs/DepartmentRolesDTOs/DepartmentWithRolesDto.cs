using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DepartmentRolesDTOs;

public class DepartmentWithRolesDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}
