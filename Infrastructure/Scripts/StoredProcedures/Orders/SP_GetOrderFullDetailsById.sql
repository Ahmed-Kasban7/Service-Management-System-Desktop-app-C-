CREATE OR ALTER PROCEDURE SP_GetOrderFullDetailsById
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ord.OrderID,
        ord.OrderNumber,
        ord.Problem,
        ord.Notes,

        CASE 
            WHEN ord.OrderState = 0 THEN N'قيد الانتظار'
            WHEN ord.OrderState = 1 THEN N'مجدول'
            WHEN ord.OrderState = 2 THEN N'جاري التنفيذ'
            WHEN ord.OrderState = 3 THEN N'مكتمل'
            WHEN ord.OrderState = 4 THEN N'ملغي'
        END AS OrderState,

        ord.StartDate,
        ord.EndedDate,

        c.Address,
        p.Name,
        d.DeviceID,
        STRING_AGG(ph.PhoneNumber, ' - ') AS PhoneNumbers,

        d.ModelName,
        d.SerialNumber,

        b.BrandName AS Brand,
        t.TypeName AS [Type],
        s.SpecName AS Spec

    FROM Orders ord

    INNER JOIN Customers c ON ord.CustomerID = c.CustomerID
    INNER JOIN Persons p ON c.PersonID = p.PersonID
    INNER JOIN Devices d ON ord.DeviceID = d.DeviceID

    LEFT JOIN Phones ph ON p.PersonID = ph.PersonID
    LEFT JOIN Brands b ON d.BrandID = b.BrandID
    LEFT JOIN Types t ON d.TypeID = t.TypeID
    LEFT JOIN Specs s ON d.SpecID = s.SpecID

    WHERE ord.OrderID = @OrderID

    GROUP BY 
        ord.OrderID,
        ord.OrderNumber,
        ord.Problem,
        ord.Notes,
        ord.StartDate,
        ord.EndedDate,
        ord.OrderState,
        c.Address,
        p.Name,
        d.DeviceID,
        d.ModelName,
        d.SerialNumber,
        b.BrandName,
        t.TypeName,
        s.SpecName;
END