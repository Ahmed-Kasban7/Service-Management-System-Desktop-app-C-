CREATE OR ALTER PROCEDURE SP_GetEmployeesLookup
    @RoleName NVARCHAR(max)
AS
BEGIN 
    SELECT 
        e.EmployeeId AS Id,
        p.Name AS Name
    FROM Employees e
    JOIN Persons p ON e.PersonID = p.PersonID
    JOIN DepartmentRoles dr ON e.RoleID = dr.RoleID
    WHERE dr.RoleName = @RoleName AND p.IsActive = 1
    ORDER BY p.Name
END