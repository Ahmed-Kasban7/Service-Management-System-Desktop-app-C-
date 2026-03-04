CREATE OR ALTER PROCEDURE SP_GetCustomerCount  
AS 
BEGIN 

    SELECT COUNT(*) AS CustomerCount 
    FROM Customers;  
END