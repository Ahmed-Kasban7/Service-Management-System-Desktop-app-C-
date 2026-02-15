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
}
