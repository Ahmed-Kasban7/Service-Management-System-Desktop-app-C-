CREATE OR ALTER PROCEDURE SP_GetRolesByDepartment
    @DepartmentId INT
AS
BEGIN
    SELECT RoleID, RoleName
    FROM DepartmentRoles
    WHERE DepartmentID = @DepartmentId
    ORDER BY RoleName
END