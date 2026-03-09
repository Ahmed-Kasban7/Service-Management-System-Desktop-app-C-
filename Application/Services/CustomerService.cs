using Application.DTOs;
using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.DTOs.CustomerDTOs;
using Application.Common;
using Application.Repositories;
namespace Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public List<CustomerSummary> GetPagedCustomerSummaries(int pageNumber , int rowPerPage)
    {
        if (pageNumber <= 0) throw new ArgumentOutOfRangeException("Page number must be greater than 0.");
        if (rowPerPage <= 0) throw new ArgumentOutOfRangeException("Rows per page must be greater than 0.");

        return _customerRepository.GetPagedCustomerSummaries(pageNumber, rowPerPage);
    }

    public int GetCustomerCount()
    {
        return _customerRepository.GetCustomerCount();
    }
    public Result DeleteCustomer(int customerId)
    {
        if (customerId < 1)
            return Result.Failure("كود العميل يجب ان يكون اكبر من 0");

        var deleted = _customerRepository.Delete(customerId);

        return deleted ? Result.Success() : Result.Failure("العميل غير موجود");
    }

    public List<CustomerSummary> SearchCustomerPagedBy(string searchWord , int pageNumber, int rowPerPage)
    {
        return _customerRepository.SearchCustomerPagedBy(searchWord, pageNumber, rowPerPage);
    }
    public int GetSearchCustomerCount(string word)
    {
        return _customerRepository.GetSearchCustomerCount(word);
    }

    public bool UpdateCustomerInfo(CustomerUpdate customerInfo)
    {
        
        Customer ? customer = _customerRepository.GetCustomerById(customerInfo.Id);

        if (customer == null)
        {
            throw new ArgumentException($"العميل رقم {customerInfo.Id} غير موجود.");
        }
        customer.UpdateDetails(customerInfo.Name , customerInfo.Age , customerInfo.Sex , customerInfo.Address , customerInfo.Discount);

        return _customerRepository.UpdateCustomerInfo(customer);
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



}
