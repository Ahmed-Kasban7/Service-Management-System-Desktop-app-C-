CREATE OR ALTER PROCEDURE SP_GetPagedOrderSummaries
@PageNumber INT,
@PageSize INT,
@TotalOrderCount INT OUTPUT   
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @TotalOrderCount = COUNT(*) FROM Orders;

    SELECT 
    ord.OrderID , 
        ord.OrderNumber,
        p.Name AS CustomerName,
        ord.StartDate,
        ord.OrderState ,
        CASE 
            WHEN ord.OrderState = 0 THEN N'قيد الانتظار'
            WHEN ord.OrderState = 1 THEN N'مجدول'
            WHEN ord.OrderState = 2 THEN N'جاري التنفيذ'
            WHEN ord.OrderState = 3 THEN N'مكتمل'
            WHEN ord.OrderState = 4 THEN N'ملغي'
        END AS State,

        c.Address,
        dbo.GetFirstPersonPhoneNumber(p.PersonId) AS CustomerPhone

    FROM Orders ord
    LEFT JOIN Customers c ON ord.CustomerId = c.CustomerId  
    JOIN Persons p ON c.PersonId = p.PersonId

    ORDER BY ord.StartDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END