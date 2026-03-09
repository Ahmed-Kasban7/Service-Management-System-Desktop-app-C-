create or alter procedure SP_SearchCustomerCount @word nvarchar(max) 
as 
begin 
select count(*)
from  Persons p  join Customers c on p.PersonID = c.PersonID 
where p.Name like '%' + @word + '%' or  p.PersonID = TRY_CAST(@word as int) 

end 