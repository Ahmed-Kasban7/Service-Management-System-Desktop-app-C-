using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Person : BaseEntity 
{
    public string Name { get;  set; }
    public int? Age { get;  set; }
    public ESex Sex { get;  set; }
    public DateTime CreatedDate { get; set ; } = DateTime.Now;

    public List<Phone> Phones { get; set; } = new ();

    public void UpdateAge(int age ) // rich domain 
    {
        if (age < 0)
            throw new ArgumentOutOfRangeException("مينفعش العمر يبقى بالسالب");

        Age = age;
    }

}
