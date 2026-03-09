create or alter procedure SP_SearchCustomerPaged @word nvarchar(max) , @PageNumber int , @RowPerPage int 
as 
begin 
select p.PersonID ,name ,c.Address , 
dbo.GetFirstPersonPhoneNumber(p.PersonID) as PhoneNumber 
from  Persons p  join Customers c on p.PersonID = c.PersonID 
where p.Name like '%' + @word + '%' or  p.PersonID = TRY_CAST(@word as int) 
order by p.PersonID

OFFSET (@PageNumber-1) * @RowPerPage ROWS 
fetch Next @RowPerPage ROWS ONLY
end 