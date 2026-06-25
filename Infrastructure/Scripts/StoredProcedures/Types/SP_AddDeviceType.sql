CREATE OR ALTER PROCEDURE SP_AddDeviceType
    @TypeName NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Types WHERE TRIM(TypeName) = TRIM(@TypeName))
    BEGIN
        SELECT 0 AS Result;
        RETURN;
    END

    INSERT INTO Types (TypeName)
    VALUES (TRIM(@TypeName));

    SELECT 1 AS Result;
END
