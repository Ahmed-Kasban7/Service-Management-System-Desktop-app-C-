CREATE OR ALTER PROCEDURE SP_DeleteCampaign
    @CampaignId INT
AS
BEGIN
    SET NOCOUNT ON;

    Update Campaigns 
    set IsActive = 0
    WHERE CampaignId = @CampaignId 
      AND NOT EXISTS (SELECT 1 FROM Customers WHERE CampaignID = @CampaignId);

    SELECT @@ROWCOUNT; 
END
