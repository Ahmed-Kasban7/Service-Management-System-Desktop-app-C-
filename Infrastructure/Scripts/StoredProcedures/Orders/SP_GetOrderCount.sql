

CREATE OR ALTER PROCEDURE SP_GetOrderCount
AS
BEGIN
    SET NOCOUNT ON;
    select count(*) from Orders
END