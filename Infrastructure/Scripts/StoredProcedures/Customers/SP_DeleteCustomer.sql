CREATE OR ALTER PROCEDURE SP_DeleteCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Orders WHERE CustomerId = @CustomerId)
    BEGIN
        ;THROW 50001, N'لا يمكن حذف هذا العميل لأن له طلبات مسجلة في النظام', 1;
        RETURN;
    END

    DECLARE @personId INT;
    SELECT @personId = PersonId FROM Customers WHERE CustomerId = @CustomerId;

    IF @personId IS NULL
    BEGIN
        RETURN;
    END

    
    UPDATE Persons 
    SET IsActive = 0 
    WHERE PersonID = @personId;

END;