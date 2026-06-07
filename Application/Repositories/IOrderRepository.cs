using Application.Common;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.OrderDTOs;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Repositories;

public interface IOrderRepository:ICommandRepository<Order> , IQueryRepository<Order>
{
    PagedResult<OrderSummaryDto> GetPagedOrderSummaries(int pageNumber, int pageSize);
    PagedResult<OrderSummaryDto> SearchOrderPage(string searchWord,int pageNumber, int pageSize);
    OrderDetailsDto GetOrderFullDetailsById(int orderId);
    bool IsOrderExists (int orderId);
    int GetOrderCount();
    IEnumerable<DeviceOrderHistoryDto> GetOrdersByDeviceId(int deviceId);
    IEnumerable<CustomerOrderSummaryDto> GetCustomerOrders(int customerId);
}
