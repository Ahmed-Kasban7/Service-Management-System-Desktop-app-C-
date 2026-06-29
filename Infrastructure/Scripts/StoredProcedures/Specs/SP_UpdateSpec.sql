CREATE OR ALTER PROCEDURE SP_UpdateSpec
    @SpecId   INT,
    @SpecName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TypeId INT;
    SELECT @TypeId = TypeID FROM Specs WHERE SpecID = @SpecId;

    IF NOT EXISTS (SELECT 1 FROM Specs WHERE SpecName = @SpecName AND TypeID = @TypeId AND SpecID <> @SpecId)
    BEGIN
        UPDATE Specs
    SET SpecName = @SpecName
    WHERE SpecID = @SpecId;

    END

    

        SELECT @@ROWCOUNT; 

END
