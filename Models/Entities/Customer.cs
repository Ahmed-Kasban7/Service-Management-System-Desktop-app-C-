using Domain.Enums;
using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Customer : Person
{
    public string Address { get;  set; }
    public int? Discount { get;  set; } = 0;

    public List<Device> CustomerDevice { get; set; } = new();
   
}
