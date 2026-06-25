    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Specs')
        BEGIN
            CREATE TABLE Specs(
                SpecID INT IDENTITY(1,1) PRIMARY KEY,
                SpecName VARCHAR(200) NOT NULL ,
                TypeID INT Not null
                    FOREIGN KEY REFERENCES Types(TypeID)  ON DELETE CASCADE
            )
        END