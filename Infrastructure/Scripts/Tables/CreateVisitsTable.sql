IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Visits')
BEGIN
    CREATE TABLE Visits (
        VisitID INT IDENTITY(1,1) PRIMARY KEY,
        AppointmentID INT NOT NULL,
        
        Notes NVARCHAR(MAX) NULL,
        ActionsTaken NVARCHAR(MAX) NULL, 
        Diagnosis NVARCHAR(MAX) NULL,
        
        TotalCost DECIMAL(18,2) NOT NULL DEFAULT 0.00, 

        CONSTRAINT FK_Visits_Appointments 
            FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentId)
    )
END
