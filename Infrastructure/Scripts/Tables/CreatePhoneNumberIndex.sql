  IF NOT EXISTS (
        SELECT * 
        FROM sys.indexes 
        WHERE name = 'PhoneNumber_index' 
          AND object_id = OBJECT_ID('Phones')
    )
    BEGIN
        CREATE NONCLUSTERED INDEX PhoneNumber_index 
        ON Phones(PhoneNumber);
    END