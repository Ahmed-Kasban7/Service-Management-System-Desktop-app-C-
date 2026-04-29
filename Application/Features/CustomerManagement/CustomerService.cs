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

    //public bool UpdateCustomerInfo(CustomerUpdateDto customerInfo)
    //{
        
    //    //Customer ? customer = _customerRepository.Get(customerInfo.Id);

    //    //if (customer == null)
    //    //{
    //    //    throw new ArgumentException($"العميل رقم {customerInfo.Id} غير موجود.");
    //    //}
    //    //customer.UpdateDetails(customerInfo.Name , customerInfo.Age , customerInfo.Sex , customerInfo.Address , customerInfo.Discount);

    //    //return _customerRepository.UpdateCustomerInfo(customer);
    //}
    

    public CustomerProfileDto GetCustomerFullProfile(int id)
    {
        return _customerRepository.GetCustomerFullProfile(id);
    }

   

}
