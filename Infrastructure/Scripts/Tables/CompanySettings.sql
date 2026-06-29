 IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CompanySettings')
 BEGIN
     CREATE TABLE CompanySettings (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(MAX) NOT NULL,
    CompanyLogo VARBINARY(MAX) NULL 
);
 END