CREATE OR ALTER PROCEDURE SP_GetPagedCustomerSummaries 
@PageNumber int ,
@RowsPerPage int
as
begin 

select p.PersonID , p.Name , c.Address ,dbo.GetFirstPersonPhoneNumber(p.PersonID) as PhoneNumber
from Persons  p join Customers c on p.PersonID = c.PersonID
where p.isDeleted =0
order by p.PersonID

OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
FETCH NEXT @RowsPerPage ROWS ONLY;

end