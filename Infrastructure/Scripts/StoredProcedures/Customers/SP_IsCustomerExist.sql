create or alter procedure SP_IsCustomerExist @customerId int 
as 
begin 
SELECT 1 
        FROM Customers c
        INNER JOIN Persons p ON c.PersonID = p.PersonID
        WHERE c.CustomerID = @CustomerId AND p.IsActive = 1
        
end 