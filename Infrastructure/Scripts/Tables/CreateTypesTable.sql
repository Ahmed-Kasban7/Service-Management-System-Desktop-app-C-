        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Types')
        BEGIN
            CREATE TABLE Types(
                TypeID INT IDENTITY(1,1) PRIMARY KEY,
                TypeName VARCHAR(100) NOT NULL UNIQUE
            )
        END