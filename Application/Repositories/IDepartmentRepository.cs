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

}
