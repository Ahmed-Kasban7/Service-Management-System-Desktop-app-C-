CREATE OR ALTER PROCEDURE SP_CreateOrder 
    @StartDate DATETIME,
    @Problem NVARCHAR(MAX),
    @Notes NVARCHAR(MAX),
    @CustomerID INT,
    @DeviceID INT,
    @OrderState TINYINT,
    @OrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Orders 
    (StartDate, Problem, Notes, CustomerID, DeviceID, OrderState)
    VALUES 
    (@StartDate, @Problem, @Notes, @CustomerID, @DeviceID, @OrderState);

    SET @OrderId = CAST(SCOPE_IDENTITY() AS INT);
END