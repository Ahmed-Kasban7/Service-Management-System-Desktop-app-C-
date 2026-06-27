IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId        INT IDENTITY(1,1) PRIMARY KEY,

        PersonID          INT NOT NULL UNIQUE,
        EmployeeNumber    AS ('EMP-' + CAST(EmployeeId AS VARCHAR)) PERSISTED,

        HireDate Date  NOT NULL,

        RoleID            INT NOT NULL,

        DepartmentID INT NOT NULL , 

        Address nvarchar(max) NULL ,

        CompensationType tinyint NOT NULL DEFAULT 0,

        -- 0 مرتب ثابت 
        -- 1 نسبه 
        -- 3 مرتب و نسبه 
        -- بالمشوار 4 

        BaseSalary        DECIMAL(10,2) NULL,       
        CommissionPercent DECIMAL(5,2)  NULL,       

        IsActive          BIT NOT NULL DEFAULT 1,

        CONSTRAINT FK_Employee_Role
            FOREIGN KEY (RoleID) REFERENCES DepartmentRoles(RoleID),

        CONSTRAINT FK_Employee_Department
            FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),

        CONSTRAINT FK_Employees_Persons
            FOREIGN KEY (PersonID) REFERENCES Persons(PersonID) ON DELETE CASCADE,

            CONSTRAINT CHK_CommissionPercent
    CHECK (CommissionPercent IS NULL OR (CommissionPercent >= 0 AND CommissionPercent <= 100))
        
    )
END