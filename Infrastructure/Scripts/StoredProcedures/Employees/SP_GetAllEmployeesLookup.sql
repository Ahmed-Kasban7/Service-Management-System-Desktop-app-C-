CREATE OR ALTER PROCEDURE SP_GetAllEmployeesLookup
AS
BEGIN 
    SELECT 
        e.EmployeeId AS Id,
        p.Name AS Name
    FROM Employees e
    JOIN Persons p ON e.PersonID = p.PersonID Where p.IsActive = 1
    ORDER BY p.Name
END