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

public class GetPagedCustomerSummariesHandler
{

    private readonly ICustomerRepository _customerRepository;

    public GetPagedCustomerSummariesHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public PagedResult<CustomerSummaryDto> Handle(int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (PageSize <= 0 || PageSize > 100)
            PageSize = 8;


        var customers = _customerRepository.GetPagedCustomerSummaries(pageNumber, PageSize);


        return customers;

    }
}
