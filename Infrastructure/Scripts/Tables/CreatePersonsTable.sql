  IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persons')
  BEGIN
            CREATE TABLE Persons(
                PersonID INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(max) NOT NULL,
                Age INT NULL CHECK (Age IS NULL OR Age > 0),
                Sex TINYINT NOT NULL check (Sex = 0 or Sex = 1),
                IsActive bit NOT NULL DEFAULT 1,
                DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
            )
END