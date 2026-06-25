IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UsedSpareParts')
BEGIN
    CREATE TABLE UsedSpareParts (
        UsedSparePartsID INT IDENTITY(1,1) PRIMARY KEY,
        PartName nvarchar(max) not null ,
        VisitID INT NOT NULL,        
        Quantity INT NOT NULL CONSTRAINT CHK_UsedSpareParts_Quantity CHECK (Quantity > 0),
        UnitPrice DECIMAL(18,2) NOT NULL CONSTRAINT CHK_UsedSpareParts_UnitPrice CHECK (UnitPrice >= 0),
        
        TotalPrice AS (Quantity * UnitPrice) PERSISTED, 

        CONSTRAINT FK_UsedSpareParts_Visits 
            FOREIGN KEY (VisitID) REFERENCES Visits(VisitID) ON DELETE CASCADE
    )
END
