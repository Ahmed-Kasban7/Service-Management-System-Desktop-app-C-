IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Visits')
BEGIN
    CREATE TABLE Visits (
        VisitID INT IDENTITY(1,1) PRIMARY KEY,
        AppointmentID INT NOT NULL,
        Diagnosis NVARCHAR(MAX) NOT NULL,
        
        ActionsTaken NVARCHAR(MAX) NULL, 
        Notes NVARCHAR(MAX) NULL,
        
        TotalCostToCustomer DECIMAL(18,2) NOT NULL DEFAULT 0.00, 
        TransportationCost DECIMAL(18,2) NOT NULL DEFAULT 0.00, 
        AmountPaid DECIMAL(18,2) NOT NULL DEFAULT 0.00,  
        RemainingAmount AS (TotalCostToCustomer - AmountPaid) PERSISTED , 

        TransportationBearer TINYINT NULL ,  -- 0= Company , 1= Employee 
        PartsTransportationCost DECIMAL(18,2) NULL DEFAULT 0.00,
        PaidByEmployeeID INT NULL,


        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

         CONSTRAINT FK_Visits_Employees
            FOREIGN KEY (PaidByEmployeeID)
            REFERENCES Employees(EmployeeID),

        CONSTRAINT UQ_Visits_AppointmentID
            UNIQUE (AppointmentID),

        CONSTRAINT CK_Visits_TotalCost
            CHECK (TotalCostToCustomer >= 0),

        CONSTRAINT CK_Visits_TransportationCost
            CHECK (TransportationCost >= 0),

        CONSTRAINT CK_Visits_AmountPaid
            CHECK (AmountPaid >= 0),

        CONSTRAINT CK_Visits_PartsTransportationCost
            CHECK (PartsTransportationCost >= 0),

        CONSTRAINT CK_Visits_AmountPaid_NotGreater
            CHECK (AmountPaid <= TotalCostToCustomer)
    )
END
