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
 


  


}
