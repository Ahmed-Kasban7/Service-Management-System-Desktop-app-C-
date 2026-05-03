CREATE OR ALTER PROCEDURE SP_SearchOrderPage
    @SearchWord NVARCHAR(100),
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
;WITH SearchCTE AS
(
    SELECT  
        ord.OrderID,
        ord.OrderNumber,
        p.Name AS CustomerName,
        ord.StartDate,

        CASE 
            WHEN ord.OrderState = 0 THEN N'قيد الانتظار'
            WHEN ord.OrderState = 1 THEN N'مجدول'
            WHEN ord.OrderState = 2 THEN N'جاري التنفيذ'
            WHEN ord.OrderState = 3 THEN N'مكتمل'
            WHEN ord.OrderState = 4 THEN N'ملغي'
        END AS State,

        c.Address,
        dbo.GetFirstPersonPhoneNumber(p.PersonId) AS CustomerPhone,

        COUNT(*) OVER() AS TotalCount

    FROM Orders ord
    INNER JOIN Customers c ON c.CustomerID = ord.CustomerID
    INNER JOIN Persons p ON p.PersonID = c.PersonID

    WHERE  
        CAST(ord.OrderID AS NVARCHAR(50)) = @SearchWord
        OR ord.OrderNumber = @SearchWord
        OR p.Name LIKE N'%' + @SearchWord + N'%'
)

SELECT *
FROM SearchCTE
ORDER BY StartDate DESC
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

END