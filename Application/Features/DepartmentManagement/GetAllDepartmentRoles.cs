using Application.DTOs.DepartmentDTOs;
using Application.DTOs.DepartmentRolesDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class GetAllDepartmentRolesHandler
{
    private readonly IDepartmentRepository _repository;

    public GetAllDepartmentRolesHandler(IDepartmentRepository repository)
        => _repository = repository;

    public IEnumerable<DepartmentWithRolesDto> Handle()
    {
        return _repository.GetAllDepartmentRoles();
    }
}
