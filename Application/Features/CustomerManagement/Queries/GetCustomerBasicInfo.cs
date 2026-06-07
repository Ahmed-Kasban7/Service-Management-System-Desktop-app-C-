using Application.DTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagement.Queries;

public class GetCustomerBasicInfoHandler
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerBasicInfoHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    
    }

    public CustomerBasicInfoDto Handle(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid customer id");

        var customer = _customerRepository.GetCustomerBasicInfo(id);

        if (customer is null)
            throw new Exception("Customer not found");

        return customer;

    }

}
