IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        EmployeeId    INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeNumber AS ('EMP-' + CAST(EmployeeId AS VARCHAR)) PERSISTED,
        PersonId      INT NOT NULL UNIQUE,
        JobId         INT NOT NULL,
        HireDate      DATE NOT NULL ,
        HasBasicSalary BIT NOT NULL DEFAULT 0,
        HasCommission  BIT NOT NULL DEFAULT 0,
        Salary        DECIMAL(10,2) ,
        CommissionRate DECIMAL(5,2),

        CONSTRAINT CHK_Salary CHECK (
            (HasBasicSalary = 1 AND Salary IS NOT NULL AND Salary >= 0) OR
            (HasBasicSalary = 0 AND Salary IS NULL)
        ),
        CONSTRAINT CHK_Commission CHECK (
            (HasCommission = 1 AND CommissionRate IS NOT NULL 
             AND CommissionRate >= 0 AND CommissionRate <= 1) OR
            (HasCommission = 0 AND CommissionRate IS NULL)
        ),

         CONSTRAINT CHK_AtLeastOneIncome CHECK (
              HasCommission = 1 OR HasBasicSalary = 1),


        CONSTRAINT FK_Employees_JobTitles
            FOREIGN KEY (JobId) REFERENCES JobTitles(JobId),
        CONSTRAINT FK_Employees_Persons
            FOREIGN KEY (PersonId) REFERENCES Persons(PersonID) ON DELETE CASCADE
    )
END