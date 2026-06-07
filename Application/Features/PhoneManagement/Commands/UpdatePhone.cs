using Application.Common;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement.Commands;

public class UpdatePhoneHandler
{
    private readonly IPhoneRepository _phoneRepository;

    public UpdatePhoneHandler(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public Result<bool> Handle(string newPhone, string oldPhone)
    {
        var phone = new Phone(newPhone);

        if (_phoneRepository.IsPhoneExist(newPhone))
            return Result<bool>.Failure("رقم الهاتف مسجل بالفعل.");

        bool result = _phoneRepository.UpdatePhone(phone.PhoneNumber, oldPhone);

        return Result<bool>.Success(result);
    }


}
