using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

public static class DatabaseInitializer
{
    private static readonly string _baseConnectionString;
    private static readonly string _databaseName;
    static DatabaseInitializer() // the first thing will do to get connection string 
    {
            var configuration = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory) 
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

        _baseConnectionString = configuration.GetConnectionString("DefaultConnection")
                 ?? throw new Exception("ملف اعدادات قاعده البيانات غير موجود ");

        _databaseName = configuration["DatabaseSettings:DatabaseName"]
           ?? throw new Exception("اسم قاعده البيانات غير موجود فى  ملف الاعدادات");
    }

    private static SqlConnection CreateConnection(string? databaseName = null)
    {
        var builder = new SqlConnectionStringBuilder(_baseConnectionString);

        if (!string.IsNullOrEmpty(databaseName))
            builder.InitialCatalog = databaseName;

        return new SqlConnection(builder.ToString());
    }

    public static SqlConnection GetConnection()
    {
        return CreateConnection(_databaseName);
    }
    private static void ExecuteScript(string script)
    {
        using var connection = CreateConnection(_databaseName);
        connection.Open();

        using var command = new SqlCommand(script, connection);
        command.ExecuteNonQuery();
    }

    private static void CreateDatabaseIfNotExist()
    {
        using var connection = CreateConnection("master");


        string Script = @"if NOT exists (select * From sys.databases where name = 'ServiceManagementSystem' )
                     begin 
                        Create Database ServiceManagementSystem ;
                      end ";


        connection.Open();

        using SqlCommand command = new SqlCommand(Script, connection);
        command.ExecuteNonQuery();

    }

    private static void CreatePersonsTable() =>
       ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persons')
        BEGIN
            CREATE TABLE Persons(
                PersonID INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(200) NOT NULL,
                Age INT NULL,
                Sex TINYINT NOT NULL,
                DateCreated DATETIME DEFAULT GETDATE(),
                IsDeleted BIT DEFAULT 0
            )
        END");

    private static void CreateCustomersTable() =>
       ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
        BEGIN
            CREATE TABLE Customers(
                PersonID INT PRIMARY KEY
                    FOREIGN KEY REFERENCES Persons(PersonID) ON DELETE CASCADE,
                Discount INT NULL,
                Address NVARCHAR(500) NOT NULL
            )
        END");

    private static void CreatePhoneNumberIndex()
    {
        ExecuteScript(@"
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
    ");
    }

    private static void CreatePhonesTable() =>
        ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Phones')
        BEGIN
            CREATE TABLE Phones(
                PhoneID INT IDENTITY(1,1) PRIMARY KEY,
                PhoneNumber NVARCHAR(11) NOT NULL UNIQUE
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
        END");

    private static void CreateTypesTable() =>
      ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Types')
        BEGIN
            CREATE TABLE Types(
                TypeID INT IDENTITY(1,1) PRIMARY KEY,
                TypeName VARCHAR(100) NOT NULL UNIQUE
            )
        END");

    private static void CreateSpecsTable() =>
     ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Specs')
        BEGIN
            CREATE TABLE Specs(
                SpecID INT IDENTITY(1,1) PRIMARY KEY,
                SpecsName VARCHAR(100) NOT NULL UNIQUE,
                TypeID INT Not null
                    FOREIGN KEY REFERENCES Types(TypeID)  ON DELETE CASCADE
            )
        END");

    private static void CreateDevicesTable() =>
       ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
        BEGIN
            CREATE TABLE Devices(
                DeviceID INT IDENTITY(1,1) PRIMARY KEY,
                SerialNumber VARCHAR(100) NULL,
                ModelName VARCHAR(200) NULL,
                CustomerID INT
                    FOREIGN KEY REFERENCES Customers(PersonID) ON DELETE CASCADE,
                BrandID INT FOREIGN KEY REFERENCES Brands(BrandID),
                TypeID INT FOREIGN KEY REFERENCES Types(TypeID),
                SpecID INT FOREIGN KEY REFERENCES Specs(SpecID)
            )
        END");
    private static void CreateBrandsTable() =>
    ExecuteScript(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Brands')
        BEGIN
            CREATE TABLE Brands(
                BrandID INT IDENTITY(1,1) PRIMARY KEY,
                BrandName VARCHAR(100) NOT NULL UNIQUE
            )
        END");

    public static void InitializeDatabase()
    {
        CreateDatabaseIfNotExist();

        CreatePersonsTable();
        CreateCustomersTable();
        CreatePhonesTable();
        CreatePhoneNumberIndex();

        CreateBrandsTable();
        CreateTypesTable();
        CreateSpecsTable();
        CreateDevicesTable();
    }
}
