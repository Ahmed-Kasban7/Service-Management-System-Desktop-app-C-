    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Phones')
        BEGIN
            CREATE TABLE Phones(
                PhoneID INT IDENTITY(1,1) PRIMARY KEY,
                PhoneNumber VARCHAR(11) NOT NULL UNIQUE
                    CHECK (
                        LEN(PhoneNumber) = 11 AND
                        (PhoneNumber LIKE '010%' OR
                         PhoneNumber LIKE '011%' OR
                         PhoneNumber LIKE '012%' OR
                         PhoneNumber LIKE '015%')
                    ),
                PersonID INT
                    FOREIGN KEY REFERENCES Persons(PersonID) ON DELETE CASCADE
            )
        END