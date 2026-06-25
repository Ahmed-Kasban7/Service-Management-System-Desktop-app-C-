CREATE OR ALTER PROCEDURE SP_SearchCampaignPaged
    @SearchWord NVARCHAR(MAX),
    @PageNumber INT,
    @RowPerPage INT,
    @TotalCampaignCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @TotalCampaignCount = COUNT(*)

    FROM Campaigns c
    LEFT JOIN Sources s
        ON c.SourceId = s.SourceId
    WHERE
        (@SearchWord IS NULL OR @SearchWord = '')
        OR c.CampaignName LIKE N'%' + @SearchWord + N'%'
        OR s.SourceName LIKE N'%' + @SearchWord + N'%';

    SELECT
    c.CampaignId,
        c.CampaignName,
        c.StartDate,
        c.EndDate,
        s.SourceName
    FROM Campaigns c
    LEFT JOIN Sources s
        ON c.SourceId = s.SourceId
    WHERE
        (@SearchWord IS NULL OR @SearchWord = '')
        OR c.CampaignName LIKE N'%' + @SearchWord + N'%'
        OR s.SourceName LIKE N'%' + @SearchWord + N'%'
    ORDER BY c.StartDate DESC

    OFFSET (@PageNumber - 1) * @RowPerPage ROWS
    FETCH NEXT @RowPerPage ROWS ONLY;
END;