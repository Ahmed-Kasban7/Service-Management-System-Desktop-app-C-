CREATE OR ALTER PROCEDURE SP_GetCustomerPhoneCount
    @customerId INT,
    @phoneCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @phoneCount = COUNT(P.PhoneNumber)
    FROM Phones P
    INNER JOIN Customers C ON P.PersonID = C.PersonID
    WHERE C.CustomerID = @customerId;
END;
