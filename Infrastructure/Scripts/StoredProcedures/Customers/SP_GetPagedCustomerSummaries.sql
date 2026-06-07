CREATE OR ALTER PROCEDURE SP_GetPagedCustomerSummaries 
@PageNumber int ,
@RowsPerPage int , 
@TotalOrderCount INT OUTPUT   

as
begin 
    SELECT @TotalOrderCount = COUNT(*) FROM Customers;

select c.CustomerID, c.CustomerNumber , p.Name , c.Address ,dbo.GetFirstPersonPhoneNumber(p.PersonID) as PhoneNumber
from Persons  p join Customers c on p.PersonID = c.PersonID
order by p.DateCreated desc

OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
FETCH NEXT @RowsPerPage ROWS ONLY;

end