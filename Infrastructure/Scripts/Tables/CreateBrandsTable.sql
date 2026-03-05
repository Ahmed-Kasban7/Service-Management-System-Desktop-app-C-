        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Brands')
        BEGIN
            CREATE TABLE Brands(
                BrandID INT IDENTITY(1,1) PRIMARY KEY,
                BrandName VARCHAR(100) NOT NULL UNIQUE
            )
        END