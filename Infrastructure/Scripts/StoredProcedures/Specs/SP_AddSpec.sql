CREATE OR ALTER PROCEDURE SP_AddSpec
    @Spec NVARCHAR(200),
    @TypeId INT
AS
BEGIN
    SET NOCOUNT ON;

   
    IF EXISTS (SELECT 1 FROM Specs WHERE TRIM(SpecName) = TRIM(@Spec) AND TypeID = @TypeId)
    BEGIN
        SELECT 0 AS Result;
        RETURN;
    END

    INSERT INTO Specs (SpecName, TypeID)
    VALUES (TRIM(@Spec), @TypeId);

    SELECT 1 AS Result;
END
