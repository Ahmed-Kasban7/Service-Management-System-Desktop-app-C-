CREATE OR ALTER PROCEDURE SP_GetDeviceOrdersHistory
    @DeviceId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ord.OrderID,
        ord.OrderNumber,
        ord.StartDate,
        ord.Problem,
        ord.OrderState,
        CASE 
            WHEN ord.OrderState = 0 THEN N'قيد الانتظار'
            WHEN ord.OrderState = 1 THEN N'مجدول'
            WHEN ord.OrderState = 2 THEN N'جاري التنفيذ'
            WHEN ord.OrderState = 3 THEN N'مكتمل'
            WHEN ord.OrderState = 4 THEN N'ملغي'
        END AS State
    FROM Orders ord
    WHERE ord.DeviceId = @DeviceId
    ORDER BY ord.StartDate DESC;
END