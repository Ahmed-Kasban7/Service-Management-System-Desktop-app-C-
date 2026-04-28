using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entities;

public record Phone
{
    public string PhoneNumber { get; private set; }
    public Phone(string phoneNumber)
    {
        PhoneValidation(phoneNumber);
    }

    public void PhoneValidation(string newPhone)
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

        this.PhoneNumber = newPhone;
    }

}

