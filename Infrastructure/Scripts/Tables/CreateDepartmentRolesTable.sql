IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DepartmentRoles')
BEGIN
    CREATE TABLE DepartmentRoles (
        RoleID  INT IDENTITY(1,1) PRIMARY KEY,
        

        RoleName nvarchar(max) NOT NULL ,
        
        DepartmentID INT NOT NULL ,

        CONSTRAINT FK_Department_Role
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) ON DELETE CASCADE
    )
END