CREATE OR ALTER PROCEDURE SP_GetOrderById 
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    Select * from Orders where OrderID = @OrderId

END