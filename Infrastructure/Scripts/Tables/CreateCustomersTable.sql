   IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
        BEGIN
            CREATE TABLE Customers(
                CustomerID INT IDENTITY(1,1) PRIMARY KEY, 

                PersonID INT NOT NULL unique ,

                CustomerNumber AS ('C-' + CAST(CustomerID AS VARCHAR)) PERSISTED, 

                Discount INT NOT NULL DEFAULT 0 
                         CONSTRAINT CHK_Customer_Discount CHECK (Discount >= 0 AND Discount <= 100),

               Address NVARCHAR(500) NOT NULL

               CONSTRAINT FK_Customers_Persons FOREIGN KEY (PersonID) REFERENCES Persons(PersonID) ON DELETE CASCADE
            )
        END

