IF NOT EXISTS (
    SELECT 1 
    FROM sys.types 
    WHERE name = 'PhoneList'
    AND is_table_type = 1
)
BEGIN
    EXEC('
        CREATE TYPE PhoneList AS TABLE
        (
            Phone VARCHAR(11)
        )
    ')
END