IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReferenceTypes')
BEGIN
    CREATE TABLE ReferenceTypes(
        TypeID   INT IDENTITY(1,1) PRIMARY KEY,
        TypeName VARCHAR(50) NOT NULL UNIQUE
    )
END


IF NOT EXISTS (SELECT * FROM ReferenceTypes WHERE TypeName = 'Employee')
    INSERT INTO ReferenceTypes (TypeName) VALUES ('Employee');

IF NOT EXISTS (SELECT * FROM ReferenceTypes WHERE TypeName = 'Customer')
    INSERT INTO ReferenceTypes (TypeName) VALUES ('Customer');

IF NOT EXISTS (SELECT * FROM ReferenceTypes WHERE TypeName = 'Campaign')
    INSERT INTO ReferenceTypes (TypeName) VALUES ('Campaign');


IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReferenceTransactions')
BEGIN
    CREATE TABLE ReferenceTransactions(
        ReferenceID   INT IDENTITY(1,1) PRIMARY KEY,
        TransactionID INT NOT NULL, 
        TypeID        INT NOT NULL, 
        
        CONSTRAINT FK_RefTrans_TreasuryTrans 
            FOREIGN KEY (TransactionID) REFERENCES TreasuryTransactions(TransactionID) ON DELETE CASCADE,
            
        CONSTRAINT FK_RefTrans_RefTypes 
            FOREIGN KEY (TypeID) REFERENCES ReferenceTypes(TypeID)
    )
END
