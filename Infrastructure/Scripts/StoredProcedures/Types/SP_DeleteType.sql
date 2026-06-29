CREATE OR ALTER PROCEDURE SP_DeleteType
    @typeId INT
AS
BEGIN 
    SET NOCOUNT ON;

    DELETE FROM Types 
    WHERE TypeID = @typeId
      AND NOT EXISTS (SELECT 1 FROM Devices WHERE TypeID = @typeId);

        SELECT @@ROWCOUNT; 

END