CREATE OR ALTER PROCEDURE SP_GetCustomerPhones
    @customerId INT
AS
BEGIN

    SELECT PhoneNumber
    FROM Phones
    WHERE PersonID = @customerId;
END