  IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persons')
  BEGIN
            CREATE TABLE Persons(
                PersonID INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(200) NOT NULL,
                Age INT NULL CHECK (Age IS NULL OR Age > 0),
                Sex TINYINT NOT NULL check (Sex = 0 or Sex = 1),
                DateCreated DATETIME  Not Null DEFAULT GETDATE(),
                IsDeleted BIT not null DEFAULT 0
            )
END