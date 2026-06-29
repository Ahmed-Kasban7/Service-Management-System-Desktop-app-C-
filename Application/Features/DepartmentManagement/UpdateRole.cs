using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class UpdateRoleHandler
{
    private readonly IDepartmentRepository _repository;

    public UpdateRoleHandler(IDepartmentRepository repository)
        => _repository = repository;

    public bool Handle(string roleName, int roleId)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException("اسم الدور مطلوب");

        if (roleId <= 0)
            throw new ArgumentException("رقم الدور غير صالح");

        return _repository.UpdateRole(roleId, roleName);
    }
}
