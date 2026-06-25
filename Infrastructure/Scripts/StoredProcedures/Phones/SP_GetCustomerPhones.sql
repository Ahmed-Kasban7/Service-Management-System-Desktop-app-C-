CREATE OR ALTER PROCEDURE SP_GetCustomerPhones
    @customerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.PhoneNumber
    FROM Phones P
    INNER JOIN Customers C
        ON P.PersonID = C.PersonID
    WHERE C.CustomerID = @customerId;

END;