CREATE OR ALTER PROCEDURE SP_AddDepartmentRole
    @RoleName NVARCHAR(max),
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 
        FROM DepartmentRoles 
        WHERE RoleName = @RoleName 
          AND DepartmentID = @DepartmentId
    )
    BEGIN
        INSERT INTO DepartmentRoles (RoleName, DepartmentID)
        VALUES (@RoleName, @DepartmentId);
    END

    SELECT @@ROWCOUNT; 
END