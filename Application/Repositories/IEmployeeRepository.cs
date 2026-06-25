using Application.DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IEmployeeRepository
{
    bool IsEmployeeExists(int id);
    IEnumerable<PersonLookupDto> GetEmployeesLookup(string roleName);
     IEnumerable<PersonLookupDto> GetAllEmployeesLookup();
}
