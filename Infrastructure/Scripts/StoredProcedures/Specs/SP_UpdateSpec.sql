CREATE OR ALTER PROCEDURE SP_UpdateSpec
    @SpecId   INT,
    @SpecName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TypeId INT;
    SELECT @TypeId = TypeID FROM Specs WHERE SpecID = @SpecId;

    IF EXISTS (SELECT 1 FROM Specs WHERE SpecName = @SpecName AND TypeID = @TypeId AND SpecID <> @SpecId)
    BEGIN
        RETURN -1;
    END

    UPDATE Specs
    SET SpecName = @SpecName
    WHERE SpecID = @SpecId;

    IF @@ROWCOUNT > 0
        return 1;
    ELSE
        return 0;
END
