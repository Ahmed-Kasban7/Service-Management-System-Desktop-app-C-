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

    //public bool AddPhone(string phoneNumber, int personId)
    //{
    //    var newPhone = new Phone(phoneNumber);

    //    if(_phoneRepository.PhoneExists(phoneNumber))
    //        throw new ArgumentException("رقم الهاتف مسجل بالفعل.");

    //    return _phoneRepository.AddPhone(newPhone.PhoneNumber, personId);
    //}
    //public bool UpdatePhone(string newPhone, string oldPhone)
    //{
    //    var Phone = new Phone(newPhone);

    //    if (_phoneRepository.PhoneExists(newPhone))
    //        throw new ArgumentException("رقم الهاتف مسجل بالفعل.");


    //    return _phoneRepository.UpdatePhone(Phone.PhoneNumber, oldPhone);
    //}


}
