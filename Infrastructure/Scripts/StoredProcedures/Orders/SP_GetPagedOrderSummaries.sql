CREATE OR ALTER PROCEDURE SP_GetPagedOrderSummaries
@PageNumber INT,
@PageSize INT,
@TotalOrderCount INT output   
AS
BEGIN
    SET NOCOUNT ON;

    select @TotalOrderCount = count(*) from orders;

    select ord.OrderNumber ,p.Name as CustomerName, ord.Problem ,ord.StartDate , ord.OrderState as state , c.Address ,dbo.GetFirstPersonPhoneNumber(p.PersonId) as CustomerPhone
    from Orders ord left join Customers c on ord.CustomerId  = c.CustomerId  join Persons p on c.PersonId = p.PersonId
    order by ord.StartDate desc

    OFFSET (@PageNumber-1) * @PageSize  ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END