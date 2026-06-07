using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.DTOs.OrderDTOs;

public class DeviceOrderHistoryDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string? Problem { get; set; }
    public string? State { get; set; }
    public int OrderState { get; set; }

    public string StatusText => State ?? "---";
    public DeviceOrderHistoryDto(int orderId, string orderNumber, string? problem, DateTime startDate, string? state, int orderState)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Problem = problem;
        Date = startDate; 
        State = state;
        OrderState = orderState;
    }
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