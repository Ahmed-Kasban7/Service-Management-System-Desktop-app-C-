using Application.DTOs.DeviceDTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDTOs;

public record OrderDetailsDto(
    int OrderId,
    string OrderNumber,
    string Problem,
    string? Notes,
    DateTime StartDate,
    DateTime? EndDate,
    byte OrderState,      
    string State,
    string CustomerName,
    string Address,
    string CustomerPhones,
    DeviceSummaryDto CustomerDevice)
{
    public string StatusBackground => OrderState switch
    {
        0 => "#FEF9C3",
        1 => "#DBEAFE",
        2 => "#DBEAFE",
        3 => "#D1FAE5",
        4 => "#FEE2E2",
        _ => "#F3F4F6"
    };

    public string StatusForeground => OrderState switch
    {
        0 => "#92400E",
        1 => "#1E40AF",
        2 => "#1E40AF",
        3 => "#065F46",
        4 => "#991B1B",
        _ => "#374151"
    };
}

