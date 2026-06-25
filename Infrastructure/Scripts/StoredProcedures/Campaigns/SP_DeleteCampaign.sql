CREATE OR ALTER PROCEDURE SP_DeleteCampaign
    @CampaignId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Campaigns 
    WHERE CampaignId = @CampaignId 
      AND NOT EXISTS (SELECT 1 FROM Customers WHERE CampaignID = @CampaignId);

    SELECT @@ROWCOUNT; 
END
