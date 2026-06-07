CREATE OR ALTER PROCEDURE SP_GetCustomerProfile
    @id INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT 
        c.CustomerNumber,
        c.CustomerID,
        p.Name,
        p.Sex,
        p.Age,
        c.Address,
        c.Discount , 
        p.DateCreated
    FROM Customers c
    INNER JOIN Persons p ON p.PersonID = c.PersonID
    WHERE c.CustomerID = @id;

END