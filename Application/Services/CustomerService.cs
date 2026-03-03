using Application.Common.Interfaces;
using Application.DTOs;
using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
namespace Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public List<CustomerSummaryDTO> GetAllCustomers()
    {
        return _customerRepository.GetAllCustomers();
    }
    public List<CustomerSummaryDTO> SearchCustomerBy(string searchWord)
    {
        return _customerRepository.SearchCustomerBy(searchWord);
    }
    public int CreateCustomer(CustomerCreateDTO customerDto)
    {
        if (customerDto == null)
            throw new ArgumentNullException(nameof(customerDto), "بيانات العميل غير موجودة.");

        if (customerDto.Phones == null || customerDto.Phones.Count ==0)
            throw new ArgumentException("يجب إدخال رقم هاتف واحد على الأقل للعميل.");

        if (customerDto.Devices == null || customerDto.Devices.Count ==0)
            throw new ArgumentException("يجب إضافة جهاز واحد على الأقل للعميل.");

        Customer newCustomer = new Customer(customerDto.Name, customerDto.Age,customerDto.Sex, customerDto.Address, customerDto.Discount);
    
        foreach (var phone in customerDto.Phones)
        {
            newCustomer.AddPhone(phone);
        }

        foreach (var device in customerDto.Devices)
        {

            newCustomer.AddDevice(new Device(device.SerialNumber, device.Model , device.BrandID , device.TypeID , device.SpecID));
        }

        return _customerRepository.CreateCustomer(newCustomer);
    }
    public CustomerProfileDTO GetCustomerFullProfile(int id)
    {
        return _customerRepository.GetCustomerFullProfile(id);
    }

    public bool DeleteCustomer(int id)
    {
        return _customerRepository.DeleteCustomer(id);
    }

    public bool UpdateCustomerInfo(CustomerUpdateDTO customerInfo)
    {
        
        Customer ? customer = _customerRepository.GetCustomerById(customerInfo.Id);
        if (customer == null)
        {
            throw new ArgumentException($"العميل رقم {customerInfo.Id} غير موجود.");
        }
        customer.UpdateDetails(customerInfo.Name , customerInfo.Age , customerInfo.Sex , customerInfo.Address , customerInfo.Discount);

        return _customerRepository.UpdateCustomerInfo(customerInfo);
    }

}
