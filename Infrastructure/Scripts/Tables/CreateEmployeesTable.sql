IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId  INT IDENTITY(1,1) PRIMARY KEY,
        
        PersonID INT NOT NULL unique ,

        EmployeeNumber AS ('EMP-' + CAST(EmployeeId AS VARCHAR)) PERSISTED,
        
        RoleID INT NOT NULL , 

        CONSTRAINT FK_Employee_Role
    FOREIGN KEY (RoleID) REFERENCES DepartmentRoles(RoleID),

  CONSTRAINT FK_Employees_Persons FOREIGN KEY (PersonID) REFERENCES Persons(PersonID) ON DELETE CASCADE


    )
END