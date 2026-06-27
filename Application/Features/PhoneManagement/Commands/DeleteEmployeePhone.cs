using Application.Common;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement.Commands;

public class DeleteEmployeePhoneHandler
{
    private readonly IPhoneRepository _phoneRepository;

    public DeleteEmployeePhoneHandler(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }


    public Result<bool> Handle(string phoneNumber, int employeeId)
    {
        if (employeeId <= 0)
            return Result<bool>.Failure("رقم الموظف غير صحيح");

        if (!_phoneRepository.IsPhoneExist(phoneNumber))
            return Result<bool>.Failure("رقم الهاتف غير موجود.");

        if (_phoneRepository.GetEmployeePhoneCount(employeeId) <= 1)
            return Result<bool>.Failure("لا يمكن حذف هذا الرقم، يجب أن يكون للموظف رقم واحد على الأقل.");

        bool result = _phoneRepository.DeletePhone(phoneNumber);

        return Result<bool>.Success(result);
    }


}
