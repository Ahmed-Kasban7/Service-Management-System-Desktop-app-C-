CREATE OR ALTER PROCEDURE SP_AddSpec
    @Spec NVARCHAR(max),
    @TypeId INT
AS
BEGIN
    SET NOCOUNT ON;

   
    IF NOT EXISTS (SELECT 1 FROM Specs WHERE SpecName = @Spec AND TypeID = @TypeId)
    BEGIN
        
    INSERT INTO Specs (SpecName, TypeID)
    VALUES (@Spec, @TypeId);

    END

    SELECT @@ROWCOUNT; 

END
