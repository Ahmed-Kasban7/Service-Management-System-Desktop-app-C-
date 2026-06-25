using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDTOs;

public record class OrderSummaryDto(int OrderId, string OrderNumber, string CustomerName, string CustomerPhone, string Address, DateTime Date, int OrderState, string StateName) {
    public string StatusBackground => OrderState switch
    {
        0 => "#FEF9C3",
        1 => "#DBEAFE",
        2 => "#E0E7FF",
        3 => "#D1FAE5",
        4 => "#FEE2E2",
        _ => "#F3F4F6"
    };

    public string StatusForeground => OrderState switch
    {
        0 => "#92400E",
        1 => "#1E40AF",
        2 => "#3730A3",
        3 => "#065F46",
        4 => "#991B1B",
        _ => "#374151"
    };

}

