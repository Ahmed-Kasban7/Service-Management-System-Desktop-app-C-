using Application.Common;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerManagement.Queries;

public class SearchCustomerPageHandler
{
    private readonly ICustomerRepository _customerRepository;

    public SearchCustomerPageHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public PagedResult<CustomerSummaryDto> Handle(string searchWord, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (pageSize <= 0 || pageSize > 100)
            pageSize = 8;

        if (string.IsNullOrWhiteSpace(searchWord))
            searchWord = "";

        var customers = _customerRepository.SearchCustomerPaged(searchWord, pageNumber, pageSize);

        return customers;
    }
}
