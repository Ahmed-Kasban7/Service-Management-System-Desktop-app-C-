CREATE OR ALTER PROCEDURE SP_UpdateCustomerInfo 
    @personId INT,
    @Name NVARCHAR(200),
    @Age INT,
    @Sex INT,
    @Address NVARCHAR(200),
    @discount INT
AS
BEGIN 
    BEGIN TRANSACTION 
    BEGIN TRY 
        UPDATE Persons 
        SET Name = @Name, Age = @Age, Sex = @Sex
        WHERE PersonID = @personId;

        UPDATE Customers 
        SET Address = @Address, Discount = @discount
        WHERE PersonID = @personId;

        COMMIT 
    END TRY 
    BEGIN CATCH 
        ROLLBACK ;
        THROW;
    END CATCH 
END