CREATE OR ALTER PROCEDURE SP_UpdateOrder 
    @OrderId INT,
    @Problem NVARCHAR(MAX),
    @Notes NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Orders 
    SET 
        Problem = @Problem,
        Notes = @Notes
    WHERE OrderId = @OrderId; 
END