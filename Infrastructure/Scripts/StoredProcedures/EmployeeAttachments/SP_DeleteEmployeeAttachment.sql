CREATE OR ALTER PROCEDURE SP_DeleteEmployeeAttachment
    @filePath NVARCHAR(max) 
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM EmployeeAttachments 
    WHERE FilePath = @filePath;
END;