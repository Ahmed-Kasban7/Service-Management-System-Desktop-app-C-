IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    CREATE TABLE Departments (
        DepartmentID  INT IDENTITY(1,1) PRIMARY KEY,
        
        DepartmentName nvarchar(max) NOT NULL   

    )
END