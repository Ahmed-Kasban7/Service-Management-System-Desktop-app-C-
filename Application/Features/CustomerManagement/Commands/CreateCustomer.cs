using Application.DTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagment.Commands;

public class CreateCustomerHandler
{
    private readonly ICustomerRepository _repository;
    public CreateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }
    public int Handle(CustomerCreateDto customerDto)
    {

        if (customerDto == null)
            throw new ArgumentNullException("بيانات العميل غير موجودة.");

        if (customerDto.Phones == null || customerDto.Phones.Count == 0)
            throw new ArgumentException("يجب إدخال رقم هاتف واحد على الأقل للعميل.");

        if (customerDto.Devices == null || customerDto.Devices.Count == 0)
            throw new ArgumentException("يجب إضافة جهاز واحد على الأقل للعميل.");

        Customer newCustomer = new Customer(customerDto.Name, customerDto.Age, customerDto.Sex, customerDto.Address, customerDto.Discount);

        foreach (var phone in customerDto.Phones)
        {
            newCustomer.AddPhone(phone);
        }

        foreach (var device in customerDto.Devices)
        {
            newCustomer.AddDevice(new Device(device.SerialNumber, device.Model, device.BrandID, device.TypeID, device.SpecID));
        }
        var id = _customerRepository.CreateCustomer(newCustomer);
        CustomerAdded?.Invoke();
        return id;
    }
}
