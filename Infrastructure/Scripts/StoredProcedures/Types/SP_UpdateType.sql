CREATE OR ALTER PROCEDURE SP_UpdateType 
    @typeId INT,
    @typeName nvarchar(200)
AS
BEGIN 
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Types WHERE TypeName = @typeName AND TypeID <> @typeId)
    BEGIN
        UPDATE Types 
    SET TypeName = @typeName
    WHERE TypeID = @typeId;
    
    END


    SELECT @@ROWCOUNT; 


END