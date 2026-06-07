using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    public bool IsEmployeeExists(int employeeId)
    {
        return false;
    }
}
