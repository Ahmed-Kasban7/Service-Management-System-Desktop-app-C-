CREATE OR ALTER PROCEDURE SP_GetPagedCampaignSummaries
    @PageNumber INT,
    @RowsPerPage INT,
    @TotalCampaignCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @TotalCampaignCount = COUNT(*)
    FROM Campaigns;

    SELECT
    c.CampaignId,
        c.CampaignName,
        c.StartDate,
        c.EndDate,
        s.SourceName
    FROM Campaigns c
    LEFT JOIN Sources s
        ON c.SourceId = s.SourceId
    ORDER BY c.StartDate DESC

    OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
    FETCH NEXT @RowsPerPage ROWS ONLY;
END;
