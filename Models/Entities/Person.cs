using Domain.Common;
using Domain.Enums;
namespace Domain.Entities;

public class Person : BaseEntity 
{
    public string Name { get; private  set; }
    public int? Age { get;private  set; }
    public ESex Sex { get;private  set; }
    public DateTime CreatedDate { get;private set; }

    private readonly HashSet<Phone> _phones = new();
    public IReadOnlySet<Phone> Phones => _phones;

    public Person() { }
    public Person(string name , int? age , ESex sex , HashSet<Phone> phones ) : this(name , age , sex)
    {
        if (phones == null || phones.Count == 0)
            throw new ArgumentException("يجب إضافة رقم هاتف واحد على الأقل");

        foreach (var p in phones)
        {
            AddPhone(p);
        }

    } // when create a new person 
    public Person(string name , int? age , ESex sex) 
    {

        SetName(name);
        SetAge(age);
        SetSex(sex);

    } // when retrive
    public void SetName(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException("الرجاء إدخال الاسم");

       

        if (!System.Text.RegularExpressions.Regex.IsMatch(Name, @"^[\p{L} ]+$"))
            throw new ArgumentException("الاسم يجب أن يحتوي على حروف فقط");

        this.Name = Name.Trim();

    }
    public void SetAge(int? age)
    {
        if (age.HasValue && age <= 0)
            throw new ArgumentException("العمر يجب أن يكون رقمًا أكبر من صفر");

        Age = age;
    }

    public void SetSex(ESex sex)
    {
        if (!Enum.IsDefined(typeof(ESex), sex))
            throw new ArgumentException("قيمة الجنس غير صالحة");

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
    public void AddPhone(Phone newphone)
    {
        if (newphone == null)
            throw new ArgumentNullException("الرقم غير صالح");

        if (!_phones.Add(newphone))
        {
            throw new InvalidOperationException("لا يمكن إدخال نفس رقم الهاتف أكثر من مرة.");
        }
    }

    protected  void UpdateDetails(string name, int? age, ESex sex)
    {
        SetName(name);
        SetAge(age);
        SetSex(sex);
    }
    //public void DeletePhone(string phone)
    //{

    //    Phone p = new Phone(phone);

    //    if (!_phones.Contains(p))
    //        throw new InvalidOperationException("رقم الهاتف غير موجود.");

    //    if (_phones.Count <= 1)
    //        throw new InvalidOperationException("لا يمكن حذف الهاتف يجب ان يكون هنا رقم واحد مسجل على الاقل.");

    //    _phones.Remove(p);

    //}
    //public void UpdatePhone(string curPhone, string newPhone)
    //{
    //    Phone curP = new Phone(curPhone);
    //    Phone newP = new Phone(newPhone);

    //    if (!_phones.Contains(curP))
    //        throw new InvalidOperationException("رقم الهاتف القديم غير موجود.");

    //    if (_phones.Contains(newP))
    //        throw new InvalidOperationException("رقم الهاتف الجديد موجود بالفعل.");

    //    _phones.Remove(curP);
    //    _phones.Add(newP);
    //}

}
