using Application.DTOs.OrderDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Queries;

public class GetDeviceOrders
{
    private readonly IOrderRepository _orderRepository;

    public GetDeviceOrders(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IEnumerable<DeviceOrderHistoryDto> Handle(int deviceId)
    {
        if (deviceId <= 0)
            throw new ArgumentException("رقم الجهاز غير صالح");

        return _orderRepository.GetOrdersByDeviceId(deviceId);
    }
}