CREATE OR ALTER PROCEDURE SP_DeleteEmployeeAttachment
    @AttachmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM EmployeeAttachments WHERE AttachmentId = @AttachmentId;
END;