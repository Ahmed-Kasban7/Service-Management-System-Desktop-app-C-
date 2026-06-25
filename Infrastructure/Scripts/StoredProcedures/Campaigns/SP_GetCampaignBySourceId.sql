CREATE OR ALTER PROCEDURE SP_GetCampaignBySourceId
    @SourceId INT
AS
BEGIN
    SET NOCOUNT ON;
    
SELECT CampaignId, CampaignName , Discount
    FROM Campaigns 
    WHERE SourceID = @SourceId 
END
