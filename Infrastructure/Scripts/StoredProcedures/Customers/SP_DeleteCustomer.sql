CREATE OR ALTER PROCEDURE SP_DeleteCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;



    IF EXISTS (SELECT 1 FROM Orders WHERE CustomerId = @CustomerId)
    BEGIN
        RAISERROR(N'لا يمكن حذف هذا العميل لأن له طلبات مسجلة في النظام', 16, 1);
        RETURN;
    END

    Declare @personId INT ;

    select @personId =  PersonId from Customers where  CustomerId = @CustomerId;

    DELETE FROM Persons WHERE PersonID = @personId;
END