CREATE OR ALTER PROCEDURE SP_CreateCampaign 
    @CampaignName NVARCHAR(max), 
    @StartDate    DATE, 
    @EndDate      DATE, 
    @SourceId     INT, 
    @Discount     INT = 0,
    @CampaignCost DECIMAL(18,2) ,
    @Note         NVARCHAR(MAX) = NULL
AS
BEGIN 
    SET NOCOUNT ON;

    INSERT INTO Campaigns (CampaignName, StartDate, EndDate, SourceID, Discount,CampaignCost, Notes)
    VALUES (@CampaignName, @StartDate, @EndDate, @SourceId, @Discount,@CampaignCost, @Note);


    SELECT SCOPE_IDENTITY();   
    
END