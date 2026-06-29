using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class AddRoleHandler
{
    private readonly IDepartmentRepository _repository;

    public AddRoleHandler(IDepartmentRepository repository)
        => _repository = repository;

    public bool Handle(string roleName , int depId) 
    {
        if(depId <=0)
            throw new ArgumentException("رقم القسم غير صالح");


        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException("اسم الدور مطلوب");



        return _repository.AddRole(roleName, depId);
    }
}
