CREATE OR ALTER PROCEDURE SP_AddCustomerPhone
    @phoneNumber VARCHAR(11),
    @personId INT
AS
BEGIN

    INSERT INTO Phones (PhoneNumber, PersonID)
    VALUES (@phoneNumber, @personId);
END