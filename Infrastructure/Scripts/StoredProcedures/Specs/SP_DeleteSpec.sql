CREATE OR ALTER PROCEDURE SP_DeleteSpec 
    @SpecId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Specs 
    WHERE SpecID = @SpecId
      AND NOT EXISTS(SELECT 1 FROM Devices WHERE SpecID = @SpecId);

    IF @@ROWCOUNT > 0
        SELECT 1 AS Result; 
    ELSE
        SELECT 0 AS Result; 
END
