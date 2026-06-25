IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TreasuryTransactions')
BEGIN
    CREATE TABLE TreasuryTransactions(
        TransactionID   INT IDENTITY(1,1) PRIMARY KEY,
        TreasuryID      INT NOT NULL DEFAULT 1,
        TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
        Amount          DECIMAL(18,2) NOT NULL,    
        TransactionType BIT NOT NULL, -- 0 = Out , 1 = IN
        BalanceBefore   DECIMAL(18,2) NOT NULL,
        BalanceAfter    DECIMAL(18,2) NOT NULL,
        CategoryID      INT NOT NULL,
        Description  nvarchar(max) NULL ,

        CONSTRAINT CK_Transactions_Amount CHECK (Amount > 0),
        CONSTRAINT FK_TreasuryTransactions_Treasuries FOREIGN KEY (TreasuryID) REFERENCES Treasuries(TreasuryID),
        CONSTRAINT FK_TreasuryTransactions_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
    )
END