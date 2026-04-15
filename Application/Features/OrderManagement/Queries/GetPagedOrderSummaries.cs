using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Queries;

public class GetPagedOrderSummariesHandler
{
    private readonly IOrderRepository _orderRepository;
    public GetPagedOrderSummariesHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public Result<PagedResult<OrderSummaryDto>>  Handle(int pageNumber, int PageSize)
    {
        if (pageNumber <= 0)
            return Result<PagedResult<OrderSummaryDto>>.Failure("رقم الصفحه غير صحيح");

        if (PageSize <= 0)
            return Result<PagedResult<OrderSummaryDto>>.Failure("عدد الصفوف غير صحيح");


        return Result<PagedResult<OrderSummaryDto>>.Success(_orderRepository.GetPagedOrderSummaries(pageNumber, PageSize));
            
    }
}
