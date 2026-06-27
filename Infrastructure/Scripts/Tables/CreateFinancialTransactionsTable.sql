IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinancialTransactions')
begin 

CREATE TABLE FinancialTransactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    TransactionType TINYINT NOT NULL, --  بدل انتقالات، 2: عهدة معلقة، 3: سُلفة، 4: عمولة حوافز number 1   
    Amount DECIMAL(18,2) NOT NULL, 
    EmployeeID INT NOT NULL,
    OrderID INT NULL,
    VisitID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status    TINYINT DEFAULT 0, 
    PaidAt           DATETIME NULL,

    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (VisitID) REFERENCES Visits(VisitID),

);

end