    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Phones')
        BEGIN
            CREATE TABLE Phones(
                PhoneID INT IDENTITY(1,1) PRIMARY KEY,

                PhoneNumber VARCHAR(11) NOT NULL UNIQUE
                   CHECK (
    LEN(PhoneNumber) = 11 AND 
    PhoneNumber NOT LIKE '%[^0-9]%' AND
    LEFT(PhoneNumber, 3) IN ('010','011','012','015')
),
                PersonID INT
                    FOREIGN KEY REFERENCES Persons(PersonID) ON DELETE CASCADE
            )
        END
