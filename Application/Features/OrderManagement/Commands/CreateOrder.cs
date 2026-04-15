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

public class CreateOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeviceRepository _deviceRepository;
    public CreateOrderHandler(
          IOrderRepository orderRepository,
          ICustomerRepository customerRepository,
          IDeviceRepository deviceRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _deviceRepository = deviceRepository;
    }
    public Result<int> Handle(CreateOrderDto orderDto )
    {
        if (orderDto == null)
            return Result<int>.Failure("بيانات الطلب غير صالحة");

        if (!_customerRepository.IsCustomerExist(orderDto.CustomerId))
            return Result<int>.Failure("العميل غير موجود");


        if (!_deviceRepository.IsDeviceExist(orderDto.DeviceId))
            return Result<int>.Failure("الجهاز غير موجود");

        Order order = new Order(orderDto.Problem , orderDto.Notes , orderDto.CustomerId , orderDto.DeviceId);

        int orderId = _orderRepository.Create(order);

        return Result<int>.Success(orderId);
    }

    
}
