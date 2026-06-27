CREATE OR ALTER PROCEDURE SP_GetEmployeeProfile
    @EmployeeID INT
AS
BEGIN 
    SET NOCOUNT ON; 

    SELECT 
        e.EmployeeID AS Id,
        e.EmployeeNumber,
        p.Name,
        p.Sex, 
        p.Age,
        e.Address,
        e.HireDate,
        d.DepartmentName,          
        r.RoleName,
        e.CompensationType,
        
        CASE e.CompensationType
            WHEN 0 THEN N'مرتب ثابت'
            WHEN 1 THEN N'نسبة / عمولة'
            WHEN 3 THEN N'مرتب + نسبة'
            WHEN 4 THEN N'بالمشوار'
            ELSE N'غير محدد'
        END AS CompensationTypeText,

        e.BaseSalary,
        e.CommissionPercent
        
    FROM Employees e
    INNER JOIN Persons p ON e.PersonID = p.PersonID
    INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
    INNER JOIN DepartmentRoles r ON e.RoleID = r.RoleID
    WHERE e.EmployeeID = @EmployeeID; 
END
