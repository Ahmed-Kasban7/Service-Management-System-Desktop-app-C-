IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Appointments')
BEGIN
    CREATE TABLE Appointments(
        AppointmentId         INT IDENTITY(1,1) PRIMARY KEY,
        OrderId               INT NOT NULL,
        TechnicianId          INT NOT NULL,
        TechnicianAssistantId INT NULL,
        DriverId              INT NULL,
        ScheduledDate         DATE NOT NULL,
        AppointmentState      TINYINT NOT NULL DEFAULT 0,
        VisitType             TINYINT NOT NULL,
        Notes                 NVARCHAR(MAX) NULL,
        CancelReason NVARCHAR(MAX) NULL

        CONSTRAINT FK_Appointments_Orders
            FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
        CONSTRAINT FK_Appointments_Technicians
            FOREIGN KEY (TechnicianId) REFERENCES Employees(EmployeeId),
        CONSTRAINT FK_Appointments_TechnicianAssistant
            FOREIGN KEY (TechnicianAssistantId) REFERENCES Employees(EmployeeId),
        CONSTRAINT FK_Appointments_Driver
            FOREIGN KEY (DriverId) REFERENCES Employees(EmployeeId),

        CONSTRAINT CHK_NoSelfAssistant CHECK (
            TechnicianAssistantId IS NULL OR TechnicianAssistantId <> TechnicianId
        ),
        CONSTRAINT CHK_NoSelfDriver CHECK (
            DriverId IS NULL OR DriverId <> TechnicianId
        ),
        CONSTRAINT CHK_VisitType CHECK (VisitType IN (0,1,2,3,4,5)), --1=> Repair , 2 => Parts Replacement  , 3 => Pickup , 4 => Delivery , 5 => Following Up
        CONSTRAINT CHK_AppointmentState CHECK (AppointmentState IN (0,1,2,3)) -- Scheduled , Overdue , Completed , Cancelled
    )
END