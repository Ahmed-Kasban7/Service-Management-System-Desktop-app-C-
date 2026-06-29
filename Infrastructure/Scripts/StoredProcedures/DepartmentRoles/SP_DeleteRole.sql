CREATE OR ALTER PROCEDURE SP_DeleteRole
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

   DELETE FROM DepartmentRoles
    WHERE RoleID = @RoleId
      AND NOT EXISTS (SELECT 1 FROM Employees WHERE RoleID = @RoleId)
      AND NOT EXISTS (
          SELECT 1 
          FROM DepartmentRoles 
          WHERE RoleID = @RoleId 
            AND (RoleName = N'مساعد فنى' OR RoleName = N'فنى' OR RoleName = N'سائق')
      );

    SELECT @@ROWCOUNT; 
END