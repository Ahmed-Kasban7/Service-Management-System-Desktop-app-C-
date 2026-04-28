using Application.Common;
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
    private readonly ICustomerRepository _customerRepository;
    private readonly IPhoneRepository _phoneRepository;

    public CreateCustomerHandler(ICustomerRepository customerRepository , IPhoneRepository phoneRepository)
    {
        _customerRepository = customerRepository;
        _phoneRepository = phoneRepository;
    }

    public Result<int> Handle(CustomerCreateDto newCustomer)
    {
        if (newCustomer == null)
            return Result<int>.Failure("بيانات العميل غير موجودة.");

        if (newCustomer.Phones == null || !newCustomer.Phones.Any())
            return Result<int>.Failure("يجب إدخال رقم هاتف واحد على الأقل");

        if (newCustomer.Devices == null || !newCustomer.Devices.Any())
            return Result<int>.Failure("يجب إدخال جهاز واحد على الأقل");

        List<Phone> customerPhones;
        List<Device> customerDevices;

        try
        {
            customerPhones = newCustomer.Phones
                .Select(p => new Phone(p))
                .ToList();

            customerDevices = newCustomer.Devices
                .Select(d => new Device(d.SerialNumber, d.Model, d.BrandID, d.TypeID, d.SpecID))
                .ToList();
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }

        // duplicate check (phones)
        if (customerPhones.Count != customerPhones.Distinct().Count())
            return Result<int>.Failure("لا يمكن تكرار نفس رقم الهاتف");

        // duplicate check (devices by serial)
        if (customerDevices.Select(d => d.SerialNumber).Distinct().Count()
            != customerDevices.Count)
        {
            return Result<int>.Failure("لا يمكن تكرار نفس Serial للجهاز");
        }

       
        // DB check phones
        var phoneNumbers = customerPhones.Select(p => p.PhoneNumber);
        var existingPhones = _phoneRepository.GetExistingPhones(phoneNumbers);

        if (existingPhones.Any())
            return Result<int>.Failure(
                $"الأرقام التالية مسجلة بالفعل: {string.Join(", ", existingPhones)}");

        // create customer
        var customer = new Customer(
            newCustomer.Name,
            newCustomer.Age,
            newCustomer.Sex,
            newCustomer.Address,
            newCustomer.Discount,
            customerDevices.ToHashSet(),
            customerPhones.ToHashSet()
        );

        var customerId = _customerRepository.Create(customer);

        return Result<int>.Success(customerId);
    }

}
