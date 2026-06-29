CREATE OR ALTER PROCEDURE SP_GetCustomersLookup
AS
BEGIN 
    SELECT 
        c.CustomerID AS Id,
        p.Name AS Name
    FROM Customers c 
    JOIN Persons p ON c.PersonID = p.PersonID where p.IsActive= 1
END