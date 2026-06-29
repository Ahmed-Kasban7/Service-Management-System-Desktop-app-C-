CREATE OR ALTER PROCEDURE SP_AddDepartment
    @DepName NVARCHAR(max) 
AS
BEGIN
    SET NOCOUNT ON;


    IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = @DepName )
    BEGIN
        INSERT INTO Departments (DepartmentName)
        VALUES (@DepName); 
    END

    SELECT @@ROWCOUNT; 
END