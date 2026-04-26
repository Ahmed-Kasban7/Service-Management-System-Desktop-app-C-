using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Commands;

public class UpdateOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    public UpdateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public Result<int> Handle(UpdateOrderDto orderDto)
    {
        if (orderDto == null)
            return Result<int>.Failure("بيانات الطلب غير صالحة");


        Order order = _orderRepository.Get(orderDto.OrderId);

        if (order is null)
            return Result<int>.Failure($"لم يتم العثور على الطلب رقم {orderDto.OrderId}");

        order.UpdateOrder(orderDto.Problem , orderDto.Notes);

        int orderId = _orderRepository.Update(order);

        return Result<int>.Success(orderId);
    }

}
