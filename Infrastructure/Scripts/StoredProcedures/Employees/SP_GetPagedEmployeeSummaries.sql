CREATE OR ALTER PROCEDURE GetPagedEmployeeSummaries
    @PageNumber INT,
    @RowsPerPage INT,          
    @TotalEmployeeCount INT OUTPUT   
AS
BEGIN 
    SET NOCOUNT ON; 

    SELECT @TotalEmployeeCount = COUNT(*) 
    FROM Employees ;

    SELECT 
        e.EmployeeId,
        e.EmployeeNumber,
        p.Name,                    
        dbo.GetFirstPersonPhoneNumber(p.PersonID) AS Phone,              
        d.DepartmentName,          
        r.RoleName
               
    FROM Employees e
    INNER JOIN Persons p ON e.PersonID = p.PersonID
    INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
    INNER JOIN DepartmentRoles r ON e.RoleID = r.RoleID
    where p.IsActive = 1
    
    ORDER BY e.EmployeeId DESC 
    OFFSET (@PageNumber - 1) * @RowsPerPage ROWS 
    FETCH NEXT @RowsPerPage ROWS ONLY;           
END
