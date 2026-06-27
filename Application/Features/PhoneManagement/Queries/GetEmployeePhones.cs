using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement.Queries;

public class GetEmployeePhonesHandler
{
    private readonly IPhoneRepository _phoneRepository;

    public GetEmployeePhonesHandler(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public IEnumerable<string> Handle(int employeeId)
    {
        if (employeeId <= 0)
            throw new ArgumentOutOfRangeException("رقم الموظف غير صالح");

        return _phoneRepository.GetEmployeePhones(employeeId);
    }
}
