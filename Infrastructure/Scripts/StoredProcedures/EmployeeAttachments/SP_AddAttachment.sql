CREATE OR ALTER PROCEDURE SP_AddEmployeeAttachment
    @EmployeeId INT,
    @AttachmentData VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO EmployeeAttachments (EmployeeId, AttachmentData)
    VALUES (@EmployeeId, @AttachmentData);


END;