CREATE OR ALTER PROCEDURE SP_AddSource
    @SourceName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;


    IF NOT EXISTS (SELECT 1 FROM Sources WHERE SourceName = @SourceName)
    BEGIN
        INSERT INTO Sources (SourceName)
        VALUES (@SourceName);
    END

    SELECT @@ROWCOUNT; 
END