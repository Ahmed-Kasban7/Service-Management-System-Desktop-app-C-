using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Queries;

public class GetOrderFullDetailsHandler
{
    IOrderRepository _orderRepository;

    public GetOrderFullDetailsHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Result<OrderDetailsDto> Handle(int orderId)
    {
        if (orderId <= 0)
            return Result<OrderDetailsDto>.Failure("رقم الاوردر غير صحيح");

        var orderDetails = _orderRepository.GetOrderFullDetailsById(orderId);

        if (orderDetails is null)
            return Result<OrderDetailsDto>.Failure("الاوردر غير موجود");


        return Result<OrderDetailsDto>.Success(orderDetails);
    }
}
