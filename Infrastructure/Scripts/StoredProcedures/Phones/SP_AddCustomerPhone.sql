CREATE OR ALTER PROCEDURE SP_AddCustomerPhone
    @phoneNumber VARCHAR(11),
    @customerId INT
AS
BEGIN
     SET NOCOUNT ON;

    Declare @personId INT 

    select @personId = PersonID from Customers where CustomerID = @customerId

    INSERT INTO Phones (PhoneNumber, PersonID)
    VALUES (@phoneNumber, @personId);
END