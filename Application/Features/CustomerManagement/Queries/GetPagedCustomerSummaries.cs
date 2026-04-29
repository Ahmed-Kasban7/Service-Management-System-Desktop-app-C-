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

    public Result<PagedResult<CustomerSummaryDto>> Handle(int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            return Result<PagedResult<CustomerSummaryDto>>.Failure("رقم الصفحه غير صحيح");

        if (PageSize <= 0)
            return Result<PagedResult<CustomerSummaryDto>>.Failure("عدد الصفوف غير صحيح");

        var customers = _customerRepository.GetPagedCustomerSummaries(pageNumber, PageSize);


        return Result<PagedResult<CustomerSummaryDto>>.Success(customers);

    }
}
