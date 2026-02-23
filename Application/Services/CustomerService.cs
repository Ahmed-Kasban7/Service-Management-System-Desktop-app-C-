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
    public List<CustomerSummaryDTO> SearchCustomerBy(string s)
    {
        return _customerRepository.SearchCustomerBy(s);
    }
    public List<CustomerSummaryDTO> CreateCustomer(Customer customer)
    {
        // validation 
        // after validation call infrastructure to create customer
        return _customerRepository.GetAllCustomers();
    }
    public CustomerProfileDTO GetCustomerFullProfile(int id)
    {
        return _customerRepository.GetCustomerFullProfile(id);
    }

    public bool DeleteCustomer(int id)
    {
        return _customerRepository.DeleteCustomer(id);
    }

    public bool UpdateCustomerInfo(int personId,CustomerUpdateDTO customerInfo)
    {
        if (string.IsNullOrEmpty(customerInfo.Name))
            throw new ArgumentNullException("الرجاء إدخال الاسم");

        if (customerInfo.Name.Length > 200)
            throw new ArgumentException("الاسم لا يمكن أن يزيد عن 200 حرف");

        if (!System.Text.RegularExpressions.Regex.IsMatch(customerInfo.Name, @"^[\p{L} ]+$"))
            throw new ArgumentException("الاسم يجب أن يحتوي على حروف فقط");

        if (customerInfo.Age <= 0)
            throw new ArgumentException("الرجاء إدخال عمر صالح أكبر من صفر");


        if (string.IsNullOrEmpty(customerInfo.Address))
            throw new ArgumentException("الرجاء إدخال العنوان");

        if (customerInfo.Address.Length > 500)
            throw new ArgumentException("العنوان لا يمكن أن يزيد عن 500 حرف");

        if (customerInfo.Discount < 0)
            throw new ArgumentException("الخصم لا يمكن أن يكون بالسالب");

        if (customerInfo.Discount > 100)
            throw new ArgumentException("الخصم لا يمكن أن يكون أكبر من 100%");

        return _customerRepository.UpdateCustomerInfo(personId, customerInfo);
    }

}
