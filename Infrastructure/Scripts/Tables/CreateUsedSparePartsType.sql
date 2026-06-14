IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'UsedSparePartsType')
BEGIN
    CREATE TYPE UsedSparePartsType AS TABLE (
        PartName NVARCHAR(max) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL
    )
END
