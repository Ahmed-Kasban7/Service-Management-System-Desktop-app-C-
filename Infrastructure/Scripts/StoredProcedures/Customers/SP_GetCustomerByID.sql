create or alter procedure SP_GetCustomerByID @customerId int 
as 
begin 
select 
p.Name, 
p.Sex ,
p.Age, 
c.Address,
c.Discount
from Persons p join Customers c on p.PersonID = c.PersonID where p.PersonID = @customerId
end 