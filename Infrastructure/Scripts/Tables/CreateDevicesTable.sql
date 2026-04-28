IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
            CREATE TABLE Devices(
        DeviceID INT IDENTITY(1,1) PRIMARY KEY,

        SerialNumber VARCHAR(100) NULL ,
        ModelName VARCHAR(200) NULL,

        CustomerID INT NOT NULL,
        BrandID INT NOT NULL,
        TypeID INT NOT NULL,
        SpecID INT NOT NULL,

        CONSTRAINT FK_Devices_Customers
            FOREIGN KEY (CustomerID)
            REFERENCES Customers(CustomerID)
            ON DELETE CASCADE,

        CONSTRAINT FK_Devices_Brands
            FOREIGN KEY (BrandID)
            REFERENCES Brands(BrandID),

        CONSTRAINT FK_Devices_Types
            FOREIGN KEY (TypeID)
            REFERENCES Types(TypeID),

        CONSTRAINT FK_Devices_Specs
            FOREIGN KEY (SpecID)
            REFERENCES Specs(SpecID)
    );
END