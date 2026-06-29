CREATE OR ALTER PROCEDURE SP_DeleteSource
    @SourceId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Sources
    WHERE SourceID = @SourceId
      AND NOT EXISTS (SELECT 1 FROM Campaigns WHERE SourceID = @SourceId ) AND NOT EXISTS (SELECT 1 FROM Customers WHERE SourceID = @SourceId );

    SELECT @@ROWCOUNT; 
END