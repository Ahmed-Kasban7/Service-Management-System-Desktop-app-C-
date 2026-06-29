CREATE OR ALTER PROCEDURE SP_AddDeviceType
    @TypeName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Types WHERE TypeName = @TypeName)
    BEGIN
            INSERT INTO Types (TypeName)
            VALUES (@TypeName);
    END

    SELECT @@ROWCOUNT; 
END
