using Application.Common;
using Application.DTOs.DepartmentRolesDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class GetRolesByDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public GetRolesByDepartmentHandler(IDepartmentRepository repository)
        => _repository = repository;

    public IEnumerable<RoleLookupDto> Handle(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("رقم القسم غير صالح");

        return  _repository.GetRolesByDepartment(departmentId);
        
    }
}