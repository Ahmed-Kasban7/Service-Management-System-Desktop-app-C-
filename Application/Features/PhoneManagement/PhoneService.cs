using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PhoneManagement;

public class PhoneService
{
    private readonly IPhoneRepository _phoneRepository;
    public PhoneService(IPhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public bool DeletePhone(string phoneNumber, int personId)
    {
        if (_phoneRepository.GetPersonPhoneCount(personId) <= 1)
            throw new InvalidOperationException("لا يمكن حذف هذا الرقم، يجب أن يكون للشخص رقم واحد على الأقل.");

        return _phoneRepository.DeletePhone(phoneNumber);
    }

    


}
