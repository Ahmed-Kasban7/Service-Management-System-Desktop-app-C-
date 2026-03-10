

IF NOT EXISTS (
    SELECT 1 
    FROM sys.types 
    WHERE name = 'DeviceList'
    AND is_table_type = 1
)
BEGIN
    EXEC('
        CREATE TYPE DeviceList AS TABLE
(
    BrandId INT,
    TypeId INT,
    SpecId INT,
    SerialNumber VARCHAR(100),
    Model NVARCHAR(200)
)

    ')
END