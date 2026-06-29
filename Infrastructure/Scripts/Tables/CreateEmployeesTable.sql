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
        -- 2 مرتب و نسبه 
        -- بالمشوار3 

        BaseSalary        DECIMAL(10,2) NULL,       
        Commission DECIMAL(10,2)  NULL,   
        CommissionType BIT NULL   , -- 0 = percent , 1 = fixed money  


        CONSTRAINT FK_Employee_Role
            FOREIGN KEY (RoleID) REFERENCES DepartmentRoles(RoleID),

        CONSTRAINT FK_Employee_Department
            FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),

        CONSTRAINT FK_Employees_Persons
            FOREIGN KEY (PersonID) REFERENCES Persons(PersonID) ON DELETE CASCADE , 

            CONSTRAINT CK_Employees_CompensationType
CHECK (CompensationType BETWEEN 0 AND 3)
        
    )
END