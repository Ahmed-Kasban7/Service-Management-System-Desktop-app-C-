        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
        BEGIN
            CREATE TABLE Devices(
                DeviceID INT IDENTITY(1,1) PRIMARY KEY,
                SerialNumber VARCHAR(100) NULL,
                ModelName VARCHAR(200) NULL,
                CustomerID INT
                    FOREIGN KEY REFERENCES Customers(CustomerID) ON DELETE CASCADE,
                BrandID INT FOREIGN KEY REFERENCES Brands(BrandID),
                TypeID INT FOREIGN KEY REFERENCES Types(TypeID),
                SpecID INT FOREIGN KEY REFERENCES Specs(SpecID)
            )
        END