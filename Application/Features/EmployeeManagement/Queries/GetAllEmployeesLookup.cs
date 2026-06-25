using Application.DTOs.PersonDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EmployeeManagement.Queries;

public class GetAllEmployeesLookupHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetAllEmployeesLookupHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public IEnumerable<PersonLookupDto> Handle()
    {
 
        return _employeeRepository.GetAllEmployeesLookup();
    }
}
