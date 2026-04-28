  IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'JobTitles')
  BEGIN
            CREATE TABLE JobTitles(
                JobId INT IDENTITY(1,1) PRIMARY KEY,
                JobName nvarchar(max) not null 
            )
END