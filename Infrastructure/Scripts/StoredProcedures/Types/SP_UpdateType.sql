CREATE OR ALTER PROCEDURE SP_UpdateType 
    @typeId INT,
    @typeName nvarchar(200)
AS
BEGIN 
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Types WHERE TypeName = @typeName AND TypeID <> @typeId)
    BEGIN
        RETURN -1; 
    END


    UPDATE Types 
    SET TypeName = @typeName
    WHERE TypeID = @typeId;
    
    IF @@ROWCOUNT > 0
        RETURN 1; 
    ELSE
        RETURN 0;

END