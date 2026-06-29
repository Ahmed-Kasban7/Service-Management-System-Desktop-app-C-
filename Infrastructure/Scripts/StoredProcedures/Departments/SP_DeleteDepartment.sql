CREATE OR ALTER PROCEDURE SP_DeleteDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    Delete From Departments
    WHERE DepartmentID = @DepartmentId
      AND NOT EXISTS (SELECT 1 FROM Employees WHERE DepartmentID = @DepartmentId) 
      AND NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentID = @DepartmentId and (DepartmentName = N'النقل' or  DepartmentName = N'الصيانة'));

    SELECT @@ROWCOUNT; 
END