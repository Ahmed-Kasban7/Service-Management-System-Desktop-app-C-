IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TransactionReferences')
BEGIN
    CREATE TABLE TransactionReferences(
        ID            INT IDENTITY(1,1) PRIMARY KEY,
        TransactionID INT NOT NULL,  
        ReferenceType INT NOT NULL , -- Customer = 1 , Employee = 2 , Order =3 , Campaign = 4 
        ReferenceID   INT NOT NULL,
        DisplayName   NVARCHAR(100) NOT NULL ,

        CONSTRAINT FK_TransactionReferences_Transactions FOREIGN KEY (TransactionID) REFERENCES TreasuryTransactions(TransactionID) ON DELETE CASCADE
    )
END