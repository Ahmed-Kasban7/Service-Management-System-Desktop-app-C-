IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sources')
BEGIN

    CREATE TABLE Sources (
        SourceID    INT IDENTITY(1,1) PRIMARY KEY,
        SourceName  NVARCHAR(max) NOT NULL , 
    );

END