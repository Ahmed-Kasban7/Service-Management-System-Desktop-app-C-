CREATE OR ALTER PROCEDURE SP_CampaignDetails 
@CampaignId INT
AS
BEGIN 
    SET NOCOUNT ON;
    
    Select 
    c.CampaignName , c.StartDate , c.EndDate , c.Discount , c.CampaignCost , c.Notes , s.SourceName
    from  Campaigns c left join Sources s on c.SourceId = s.SourceId
    where c.CampaignId = @CampaignId

END