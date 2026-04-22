using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagement.Queries;

public class GetCustomersLookupHandler
{
    private readonly ICustomerRepository _customerRepository;
    public GetCustomersLookupHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<CustomerLookupDto> Handle()
    {
       return  _customerRepository.GetCustomersLookup();
    }
}
