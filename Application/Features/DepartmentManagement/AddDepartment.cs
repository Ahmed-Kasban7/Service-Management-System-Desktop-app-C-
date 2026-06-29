using Application.DTOs.DepartmentDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class AddDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public AddDepartmentHandler(IDepartmentRepository repository)
        => _repository = repository;

    public bool Handle(string depName)
    {
        if(string.IsNullOrWhiteSpace(depName))
            throw new ArgumentException("اسم القسم مطلوب");

        return _repository.AddDepartment(depName);
    }
}
