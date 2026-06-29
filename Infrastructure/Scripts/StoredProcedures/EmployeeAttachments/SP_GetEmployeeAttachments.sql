CREATE OR ALTER PROCEDURE SP_GetEmployeeAttachments
    @employeeId INT 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        AttachmentID AS Id,
        AttachmentData
    FROM EmployeeAttachments
    WHERE EmployeeID = @employeeId

END;
