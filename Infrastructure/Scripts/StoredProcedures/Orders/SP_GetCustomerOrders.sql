CREATE OR ALTER PROCEDURE SP_GetCustomerOrders
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ord.OrderID,
        ord.OrderNumber,
        ord.StartDate,
        CASE 
            WHEN ord.OrderState = 0 THEN N'قيد الانتظار'
            WHEN ord.OrderState = 1 THEN N'مجدول'
            WHEN ord.OrderState = 2 THEN N'جاري التنفيذ'
            WHEN ord.OrderState = 3 THEN N'مكتمل'
            WHEN ord.OrderState = 4 THEN N'ملغي'
        END AS State,
        ord.OrderState,
        
        CONCAT_WS(' ', dt.TypeName, db.BrandName, ds.SpecName) AS DeviceName
        
    FROM Orders ord
    LEFT JOIN Devices d       ON ord.DeviceId  = d.DeviceId
    LEFT JOIN Types dt  ON d.TypeID      = dt.TypeID
    LEFT JOIN Brands db ON d.BrandID     = db.BrandID
    LEFT JOIN Specs ds  ON d.SpecID      = ds.SpecID
    WHERE ord.CustomerId = @CustomerId
    ORDER BY ord.StartDate DESC;
END