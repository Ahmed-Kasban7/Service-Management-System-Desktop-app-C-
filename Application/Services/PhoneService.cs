using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

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

    public bool AddPhone(string phoneNumber, int personId)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("رقم الهاتف مطلوب.");

        if (!phoneNumber.All(char.IsDigit))
            throw new ArgumentException("رقم الهاتف يجب أن يحتوي على أرقام فقط.");

        if (phoneNumber.Length != 11)
            throw new ArgumentException("رقم الهاتف يجب أن يكون مكونًا من 11 رقمًا.");

        if (!(phoneNumber.StartsWith("010") ||
              phoneNumber.StartsWith("011") ||
              phoneNumber.StartsWith("012") ||
              phoneNumber.StartsWith("015")))
            throw new ArgumentException("رقم الهاتف يجب أن يبدأ بـ 010 أو 011 أو 012 أو 015.");

        if(_phoneRepository.PhoneExists(phoneNumber))
            throw new ArgumentException("رقم الهاتف مسجل بالفعل.");

        return _phoneRepository.AddPhone(phoneNumber, personId);
    }
    public bool UpdatePhone(string newPhone, string oldPhone)
    {
        if (string.IsNullOrWhiteSpace(newPhone))
            throw new ArgumentException("رقم الهاتف مطلوب.");

        if (!newPhone.All(char.IsDigit))
            throw new ArgumentException("رقم الهاتف يجب أن يحتوي على أرقام فقط.");

        if (newPhone.Length != 11)
            throw new ArgumentException("رقم الهاتف يجب أن يكون مكونًا من 11 رقمًا.");

        if (!(newPhone.StartsWith("010") ||
              newPhone.StartsWith("011") ||
              newPhone.StartsWith("012") ||
              newPhone.StartsWith("015")))
            throw new ArgumentException("رقم الهاتف يجب أن يبدأ بـ 010 أو 011 أو 012 أو 015.");

        if(_phoneRepository.PhoneExists(newPhone))
            throw new ArgumentException("رقم الهاتف مسجل بالفعل.");


        return _phoneRepository.UpdatePhone(newPhone, oldPhone);
    }


}
