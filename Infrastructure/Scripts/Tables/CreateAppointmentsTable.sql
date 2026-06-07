IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Appointments')
BEGIN
    CREATE TABLE Appointments(
        AppointmentId INT IDENTITY(1,1) PRIMARY KEY,

        OrderId INT NOT NULL, 
        TechnicianId INT NOT NULL,
        TechnicianAssistantId INT NULL,
        ScheduledDate DATE NOT NULL ,
        AppointmentState BIT NOT NULL DEFAULT 0,
        VisitType TINYINT NOT NULL,

        Notes NVARCHAR(MAX) NULL,

        CONSTRAINT FK_Appointments_Orders
            FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE , 

        CONSTRAINT FK_Appointments_Technicians
            FOREIGN KEY (TechnicianId) REFERENCES Employees(EmployeeId),

        CONSTRAINT FK_Appointments_TechnicianAssistant
            FOREIGN KEY (TechnicianAssistantId) REFERENCES Employees(EmployeeId) ,

          CONSTRAINT CHK_NoSelfAssistant CHECK (
            TechnicianAssistantId IS NULL OR TechnicianAssistantId <> TechnicianId
        ),

        CONSTRAINT CHK_VisitType CHECK (VisitType IN (0,1)), --0 =>  Diagnostic  ,1=> Repair 
        CONSTRAINT CHK_AppointmentState CHECK (AppointmentState IN (0,1, 2)), -- Scheduled , Completed , Cancelled
    )
END