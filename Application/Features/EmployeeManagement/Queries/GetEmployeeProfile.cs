using Application.DTOs.EmployeeDTOs;
using Application.DTOs.PersonDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Queries;

public class GetEmployeeProfileHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeProfileHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public EmployeeProfileDto Handle(int id)
    {
        if (id <= 0)
            throw new ArgumentException("رقم الموظف غير صالح");

        return _employeeRepository.GetEmployeeProfileById(id);
    }
}
