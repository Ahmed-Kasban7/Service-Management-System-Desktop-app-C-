CREATE OR ALTER PROCEDURE SP_GetCurrentBalance
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
    ISNULL(CurrentBalance, 0.00) AS CurrentBalance ,
    LastUpdated
    FROM Treasuries
    WHERE TreasuryID = 1;
END;
