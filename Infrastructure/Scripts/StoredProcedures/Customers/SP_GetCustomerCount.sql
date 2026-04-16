CREATE OR ALTER PROCEDURE SP_GetCustomerCount  
AS 
BEGIN 

    SELECT COUNT(*) AS CustomerCount 
    FROM Customers c join Persons p on c.personId = p.personId  ;  
END