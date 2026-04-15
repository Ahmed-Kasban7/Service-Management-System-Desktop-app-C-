create or alter procedure SP_IsCustomerExist @customerId int 
as 
begin 
select 1 from Customers where CustomerID = @customerId
end 