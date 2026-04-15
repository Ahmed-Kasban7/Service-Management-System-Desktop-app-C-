CREATE OR ALTER PROCEDURE SP_UpdateOrder 
    @OrderId INT,
    @Problem NVARCHAR(MAX),
    @Notes NVARCHAR(MAX),
    @OrderState TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Orders 
    SET 
        Problem = @Problem,
        Notes = @Notes,
        OrderState = @OrderState
    WHERE OrderId = @OrderId; 
END