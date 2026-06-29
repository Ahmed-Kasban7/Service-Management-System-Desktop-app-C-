CREATE OR ALTER PROCEDURE SP_UpdateCustomerInfo 
    @customerId INT,
    @Name NVARCHAR(max),
    @Age INT,
    @Sex INT,
    @Address NVARCHAR(max),
    @discount INT
AS
BEGIN 
    BEGIN TRANSACTION 
    BEGIN TRY 

        DECLARE @personId INT;

        SELECT @personId = PersonID
        FROM Customers
        WHERE CustomerID = @customerId; 

        UPDATE Customers 
        SET Address = @Address, Discount = @discount
        WHERE CustomerID = @customerId;

        UPDATE Persons 
        SET Name = @Name, Age = @Age, Sex = @Sex
        WHERE PersonID = @personId;

        COMMIT 
    END TRY 
    BEGIN CATCH 
        ROLLBACK ;
        THROW;
    END CATCH 
END