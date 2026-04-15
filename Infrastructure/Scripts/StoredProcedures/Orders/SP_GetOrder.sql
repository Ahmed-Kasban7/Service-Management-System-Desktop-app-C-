CREATE OR ALTER PROCEDURE SP_GetOrder 
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    Select * from Orders

END