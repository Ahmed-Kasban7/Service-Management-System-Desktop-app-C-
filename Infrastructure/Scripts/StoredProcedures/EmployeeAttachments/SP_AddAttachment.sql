CREATE OR ALTER PROCEDURE SP_AddEmployeeAttachment
    @employeeId INT,
    @filePath NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON; 
  
    INSERT INTO EmployeeAttachments (EmployeeID, FilePath)
    VALUES (@employeeId, @filePath);

END