using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Queries;

public class SearchOrderPageHandler
{
    private readonly IOrderRepository _orderRepository;

    public SearchOrderPageHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public PagedResult<OrderSummaryDto> Handle(string searchWord, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            pageNumber = 1;

        if (pageSize <= 0 || pageSize > 100)
            pageSize = 8;

        if (string.IsNullOrWhiteSpace(searchWord))
            searchWord = "";

        var orders = _orderRepository.SearchOrderPage(searchWord, pageNumber, pageSize);

        return  orders;
    }
}
