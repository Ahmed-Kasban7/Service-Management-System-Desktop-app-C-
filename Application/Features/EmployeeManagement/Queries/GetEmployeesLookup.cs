using Application.DTOs.PersonDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Queries;

public class GetEmployeesLookupHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeesLookupHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public IEnumerable<PersonLookupDto> Handle(string roleName)
    {
        if(string.IsNullOrEmpty( roleName))
            Enumerable.Empty<PersonLookupDto>();

        return _employeeRepository.GetEmployeesLookup(roleName);
    }
}
