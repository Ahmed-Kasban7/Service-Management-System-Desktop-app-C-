using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderManagement.Queries
{
    public class GetCustomerOrdersHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetCustomerOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<CustomerOrderSummaryDto> Handle(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("رقم العميل غير صالح");

            return _orderRepository.GetCustomerOrders(customerId);
        }
    }
}
