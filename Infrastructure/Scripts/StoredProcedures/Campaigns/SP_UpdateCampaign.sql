CREATE OR ALTER PROCEDURE SP_UpdateCampaign
    @CampaignId   INT,
    @CampaignName NVARCHAR(250),
    @StartDate    DATE,
    @EndDate      DATE,
    @SourceId     INT,
    @Discount     INT,
    @Notes        NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Campaigns
    SET CampaignName = @CampaignName,
        StartDate = @StartDate,
        EndDate = @EndDate,
        SourceID = @SourceId,
        Discount = @Discount,
        Notes = @Notes
    WHERE CampaignId = @CampaignId;

    SELECT @@ROWCOUNT;
END
