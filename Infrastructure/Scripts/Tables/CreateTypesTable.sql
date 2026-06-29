        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Types')
        BEGIN
            CREATE TABLE Types(
                TypeID INT IDENTITY(1,1) PRIMARY KEY,
                TypeName VARCHAR(max) NOT NULL 
            )
        END