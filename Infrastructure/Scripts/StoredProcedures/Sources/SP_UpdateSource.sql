CREATE OR ALTER PROCEDURE SP_UpdateSource
    @SourceId INT,
    @SourceName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;

   
    IF NOT EXISTS (
        SELECT 1 
        FROM Sources 
        WHERE SourceName = @SourceName 
          AND SourceID <> @SourceId
    )
    BEGIN
        UPDATE Sources
        SET SourceName = @SourceName
        WHERE SourceID = @SourceId;
    END

    SELECT @@ROWCOUNT; 
END