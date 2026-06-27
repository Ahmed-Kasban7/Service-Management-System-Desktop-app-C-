CREATE OR ALTER PROCEDURE SP_GetDepartmentsLookup
AS
BEGIN
    SELECT DepartmentID, DepartmentName
    FROM Departments
    ORDER BY DepartmentName
END