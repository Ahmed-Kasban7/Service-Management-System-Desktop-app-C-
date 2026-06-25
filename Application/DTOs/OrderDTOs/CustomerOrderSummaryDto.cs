public class CustomerOrderSummaryDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime StartDate { get; set; }
    public string? State { get; set; }
    public int OrderState { get; set; }

    public string? DeviceName { get; set; }

    public string DeviceSummary => DeviceName ?? "---";

    public string StatusText => State ?? "---";

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