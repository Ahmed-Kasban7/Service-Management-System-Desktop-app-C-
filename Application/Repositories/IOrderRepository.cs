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
    PagedResult<OrderSummaryDto> GetPagedOrderSummaries(int pageNumber, int rowPerPage);
    OrderDetailsDto GetOrderFullDetailsById(int orderId);
    int GetOrderCount();

}
