using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Features.CustomerManagment;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

   // public event Action CustomerAdded;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<CustomerSummaryDto> GetPagedCustomerSummaries(int pageNumber , int rowPerPage)
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

    public List<CustomerSummaryDto> SearchCustomerPagedBy(string searchWord , int pageNumber, int rowPerPage)
    {
        return _customerRepository.SearchCustomerPagedBy(searchWord, pageNumber, rowPerPage);
    }
    public int GetSearchCustomerCount(string word)
    {
        return _customerRepository.GetSearchCustomerCount(word);
    }

    public bool UpdateCustomerInfo(CustomerUpdateDto customerInfo)
    {
        
        Customer ? customer = _customerRepository.Get(customerInfo.Id);

        if (customer == null)
        {
            throw new ArgumentException($"العميل رقم {customerInfo.Id} غير موجود.");
        }
        customer.UpdateDetails(customerInfo.Name , customerInfo.Age , customerInfo.Sex , customerInfo.Address , customerInfo.Discount);

        return _customerRepository.UpdateCustomerInfo(customer);
    }
    

    public CustomerProfileDto GetCustomerFullProfile(int id)
    {
        return _customerRepository.GetCustomerFullProfile(id);
    }

    public void CreateCustomer(CustomerCreateDto customerDto)
    {
        if (customerDto == null)
            throw new ArgumentNullException("بيانات العميل غير موجودة.");

        if (customerDto.Phones == null || customerDto.Phones.Count == 0)
            throw new ArgumentException("يجب إدخال رقم هاتف واحد على الأقل للعميل.");

        if (customerDto.Devices == null || customerDto.Devices.Count == 0)
            throw new ArgumentException("يجب إضافة جهاز واحد على الأقل للعميل.");

        var CustomerPhones = new HashSet<Phone>();

        foreach (var p in customerDto.Phones)
        {
            var phone = new Phone(p);

            CustomerPhones.Add(phone);
        }
        var CustomerDevices = new HashSet<Device>();
        foreach (var device in customerDto.Devices)
        {
            CustomerDevices.Add(new Device(device.SerialNumber, device.Model, device.BrandID, device.TypeID, device.SpecID));
        }

        Customer newCustomer = new Customer(customerDto.Name, customerDto.Age, customerDto.Sex, customerDto.Address, customerDto.Discount , CustomerDevices, CustomerPhones);

        

        _customerRepository.Create(newCustomer);
        ////CustomerAdded?.Invoke();
        //return id;
    }


}
