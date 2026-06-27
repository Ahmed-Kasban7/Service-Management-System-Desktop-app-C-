using Application.Common;
using Application.DTOs.DepartmentDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.DepartmentManagement;

public class GetDepartmentsLookupHandler
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentsLookupHandler(IDepartmentRepository repository)
        => _repository = repository;

    public IEnumerable<DepartmentLookupDto> Handle()
    {
          return _repository.GetDepartmentsLookup();
    }
}