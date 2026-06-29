CREATE OR ALTER PROCEDURE SP_UpdateDepartment
    @DepartmentId INT,
    @DepartmentName NVARCHAR(max) 
AS
BEGIN
    SET NOCOUNT ON;


    IF NOT EXISTS (
        SELECT 1 
        FROM Departments 
        WHERE DepartmentID = @DepartmentId 
          AND (DepartmentName = N'النقل' OR DepartmentName = N'الصيانة')
    )
    AND NOT EXISTS (
        SELECT 1 
        FROM Departments 
        WHERE DepartmentName = @DepartmentName 
          AND DepartmentID <> @DepartmentId
    )
    BEGIN
        UPDATE Departments
        SET DepartmentName = @DepartmentName
        WHERE DepartmentID = @DepartmentId;
    END

    SELECT @@ROWCOUNT; 
END