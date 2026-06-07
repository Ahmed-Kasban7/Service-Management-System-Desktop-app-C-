IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId  INT IDENTITY(1,1) PRIMARY KEY
    )
END