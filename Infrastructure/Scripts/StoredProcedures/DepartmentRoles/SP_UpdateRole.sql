CREATE OR ALTER PROCEDURE SP_UpdateRole
    @RoleId INT,
    @RoleName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 
        FROM DepartmentRoles 
        WHERE RoleID = @RoleId 
          AND (RoleName = N'مساعد فنى' OR RoleName = N'فنى' OR RoleName = N'سائق')
    )
    AND NOT EXISTS (
        SELECT 1 
        FROM DepartmentRoles r1
        JOIN DepartmentRoles r2 ON r1.DepartmentID = r2.DepartmentID
        WHERE r1.RoleID = @RoleId 
          AND r2.RoleName = @RoleName 
          AND r2.RoleID <> @RoleId
    )
    BEGIN
        UPDATE DepartmentRoles
        SET RoleName = @RoleName
        WHERE RoleID = @RoleId;
    END

    SELECT @@ROWCOUNT; 
END