IF NOT EXISTS (
    SELECT 1 
    FROM sys.types 
    WHERE name = 'AttachmentList'
    AND is_table_type = 1
)
BEGIN
    CREATE TYPE AttachmentList AS TABLE
    (
        AttachmentData    VARBINARY(MAX) NOT NULL
    )
END