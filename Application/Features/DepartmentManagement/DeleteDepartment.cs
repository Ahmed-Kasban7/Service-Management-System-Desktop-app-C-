using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class DeleteDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public DeleteDepartmentHandler(IDepartmentRepository repository)
        => _repository = repository;

    public bool Handle(int id)
    {
        if (id <= 0)
            throw new ArgumentException("رقم القسم غير صالح");

        return _repository.DeleteDepartment(id);
    }
}
