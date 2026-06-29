using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class UpdateDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentHandler(IDepartmentRepository repository)
        => _repository = repository;

    public bool Handle(string depName , int id)
    {
        if (string.IsNullOrWhiteSpace(depName))
            throw new ArgumentException("اسم القسم مطلوب");

        if (id <= 0)
            throw new ArgumentException("رقم القسم غير صالح");

        return _repository.UpdateDepartment(id,depName);
    }
}
