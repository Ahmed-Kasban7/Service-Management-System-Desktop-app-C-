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
}
