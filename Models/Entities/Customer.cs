using Domain.Enums;
using System.Collections.ObjectModel;

namespace Domain.Entities;

public class Customer : Person
{
    public string Address { get; private set; }
    public int Discount { get; private set; }
    public string CustomerNumber { get; private set; }

    private readonly HashSet<Device> _customerDevice;

    public IReadOnlySet<Device> Devices => _customerDevice;
    public Customer(string name , int ?age , ESex sex , string address , int discount ): base(name , age , sex )
    {
        UpdateAddress(address);
        UpdateDiscount(discount);
        _customerDevice = new();
    } // create new customer constructor
    public Customer(int id ,string name , int ?age , ESex sex , string address , int ?discount ): base( id ,name , age , sex )
    {
        UpdateAddress(address);
        UpdateDiscount(discount);
        _customerDevice = new();
    }

    public void UpdateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("الرجاء إدخال العنوان", nameof(address));

        if (address.Length > 500)
            throw new ArgumentException("العنوان لا يمكن أن يزيد عن 500 حرف", nameof(address));

        this.Address = address.Trim();
    }
    public void UpdateDiscount(int ?discount)
    {
        if (discount.HasValue && discount < 0)
            throw new ArgumentException("الخصم لا يمكن أن يكون بالسالب", nameof(discount));

        if (discount.HasValue && discount > 100)
            throw new ArgumentException("الخصم لا يمكن أن يكون أكبر من 100%", nameof(discount));

        Discount = discount ?? 0;
    }
    public void UpdateDetails(string name, int? age, ESex sex, string address, int? discount)
    {
        base.UpdateDetails(name, age, sex);
        UpdateAddress(address);
        UpdateDiscount(discount);
    }
    public void AddDevice(Device device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        _customerDevice.Add(device);
    }
    
}
