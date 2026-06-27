IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EmployeeAttachments')
BEGIN
    CREATE TABLE EmployeeAttachments (
        AttachmentId    INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId      INT NOT NULL,

        FilePath        NVARCHAR(max) NOT NULL,

        CONSTRAINT FK_EmployeeAttachments_Employee
            FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId) ON DELETE CASCADE
    )
END