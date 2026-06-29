using Application.DTOs.DepartmentDTOs;
using Application.DTOs.DepartmentRolesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IDepartmentRepository
{
     IEnumerable<DepartmentLookupDto> GetDepartmentsLookup();
    IEnumerable<RoleLookupDto> GetRolesByDepartment(int departmentId);
    IEnumerable<DepartmentWithRolesDto> GetAllDepartmentRoles();
    bool UpdateDepartment(int departmentId, string departmentName); 
    bool DeleteDepartment(int departmentId);
    bool AddDepartment(string departmentName);

    bool AddRole(string roleName, int departmentId);
    bool UpdateRole(int roleId, string roleName);
    bool DeleteRole(int roleId);
}
