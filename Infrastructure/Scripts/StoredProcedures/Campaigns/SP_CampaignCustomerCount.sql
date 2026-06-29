CREATE OR ALTER PROCEDURE SP_CampaignCustomerCount
    @CampaignId INT,
    @CustomerCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @CustomerCount = COUNT(CustomerID)
    FROM Customers 
    WHERE CampaignID = @CampaignId;

    SET @CustomerCount = ISNULL(@CustomerCount, 0);
END