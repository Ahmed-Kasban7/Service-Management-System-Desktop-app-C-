   IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
        BEGIN
            CREATE TABLE Customers(
                PersonID INT PRIMARY KEY
                    FOREIGN KEY REFERENCES Persons(PersonID) ON DELETE CASCADE,
                Discount INT NULL,
                Address NVARCHAR(500) NOT NULL
            )
        END