CREATE OR ALTER PROCEDURE SP_SearchEmployee
    @PageNumber INT,
    @RowsPerPage INT,      
    @SearchWord NVARCHAR(100),
    @TotalEmployeeCount INT OUTPUT   
AS
BEGIN 
    SET NOCOUNT ON; 

   SELECT @TotalEmployeeCount = COUNT(*) 
    FROM Employees e 
    INNER JOIN Persons p ON p.PersonID = e.PersonID
    WHERE p.IsActive = 1 AND (
        CAST(e.EmployeeID AS NVARCHAR(50)) = @SearchWord   
        OR e.EmployeeNumber = @SearchWord 
        OR p.Name LIKE '%' + @SearchWord + '%'
    );

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
    WHERE p.IsActive = 1 AND (
        CAST(e.EmployeeID AS NVARCHAR(50)) = @SearchWord   
        OR e.EmployeeNumber = @SearchWord 
        OR p.Name LIKE '%' + @SearchWord + '%'
    )    
    ORDER BY e.EmployeeId DESC 

    OFFSET (@PageNumber - 1) * @RowsPerPage ROWS 
    FETCH NEXT @RowsPerPage ROWS ONLY;           
END
