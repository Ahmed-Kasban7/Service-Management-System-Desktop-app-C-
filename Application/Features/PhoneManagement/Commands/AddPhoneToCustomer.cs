using Application.Common;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement.Commands;

public class AddPhoneToCustomer
{
    private readonly IPhoneRepository _phoneRepository;

    public AddPhoneToCustomer(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public Result<bool> Handle(string phoneNumber, int personId)
    {
        if (personId <= 0)
            return Result<bool>.Failure("رقم الشخص غير صحيح");

        var newPhone = new Phone(phoneNumber); 

        if (_phoneRepository.IsPhoneExist(phoneNumber))
            return Result<bool>.Failure("رقم الهاتف مسجل بالفعل");

        bool result = _phoneRepository.AddPhone(phoneNumber, personId);

        return Result<bool>.Success(result);
    }


}
