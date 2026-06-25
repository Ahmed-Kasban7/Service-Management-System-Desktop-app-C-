IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Treasuries')
BEGIN
    CREATE TABLE Treasuries(
        TreasuryID     INT IDENTITY(1,1) PRIMARY KEY,
        TreasuryName   NVARCHAR(100) NOT NULL,
        CurrentBalance DECIMAL(18,2) NOT NULL DEFAULT 0.00,    
        LastUpdated    DATETIME NOT NULL DEFAULT GETDATE()
    )
END