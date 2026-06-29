CREATE OR ALTER PROCEDURE SP_GetAllDepartmentRoles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        d.DepartmentID,
        d.DepartmentName,
        r.RoleID,
        r.RoleName
    FROM Departments d
    Inner JOIN DepartmentRoles r ON d.DepartmentID = r.DepartmentID
    ORDER BY d.DepartmentName, r.RoleName;
END