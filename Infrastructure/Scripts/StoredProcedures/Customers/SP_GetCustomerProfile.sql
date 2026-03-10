CREATE OR ALTER PROCEDURE SP_GetCustomerProfile
    @id INT
AS
BEGIN

    SELECT 
        p.PersonID,
        p.Name,
        p.Sex,
        p.Age,
        c.Address,
        c.Discount
    FROM Persons p
    INNER JOIN Customers c ON p.PersonID = c.PersonID
    WHERE p.PersonID = @id;

END