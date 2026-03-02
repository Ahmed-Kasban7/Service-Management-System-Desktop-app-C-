using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Person : BaseEntity 
{
    public string Name { get; private  set; }
    public int? Age { get;private  set; }
    public ESex Sex { get;private  set; }
    public DateTime CreatedDate { get;private set; }


    private readonly HashSet<Phone> _phones = new();
    public IReadOnlySet<Phone> Phones => _phones;
    public Person(string name , int? age , ESex sex)
    {
        UpdateName(name);
        UpdateAge(age);
        UpdateSex(sex);
        _phones = new();
        CreatedDate = DateTime.Now;
    }

    public void UpdateName(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException( "الرجاء إدخال الاسم");

        if (Name.Length > 200)
            throw new ArgumentException("الاسم لا يمكن أن يزيد عن 200 حرف");

        if (!System.Text.RegularExpressions.Regex.IsMatch(Name, @"^[\p{L} ]+$"))
            throw new ArgumentException("الاسم يجب أن يحتوي على حروف فقط");

        this.Name=Name.Trim();

    }
    public void UpdateAge(int? age)
    {
        if (age.HasValue && age <= 0)
            throw new ArgumentException("العمر يجب أن يكون رقمًا أكبر من صفر", nameof(age));

        Age = age;
    }

    public void UpdateSex(ESex sex)
    {
        if (!Enum.IsDefined(typeof(ESex), sex))
            throw new ArgumentException("قيمة الجنس غير صالحة", nameof(sex));

        this.Sex = sex;
    }

    public void AddPhone(string newphone)
    {
        Phone phone = new Phone(newphone);

        if (!_phones.Add(phone))
        {
            throw new InvalidOperationException("لا يمكن إدخال نفس رقم الهاتف أكثر من مرة.");
        }
    }

    protected  void UpdateDetails(string name, int? age, ESex sex)
    {
        UpdateName(name);
        UpdateAge(age);
        UpdateSex(sex);
    }
    public void DeletePhone(string phone)
    {

    }
    public void UpdatePhone(string phone)
    {

    }

}
