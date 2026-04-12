using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

public static class DatabaseInitializer
{
    private static readonly string _baseConnectionString;
    private static readonly string _databaseName;
    static DatabaseInitializer() 
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
    private static void ExecuteScript(string scriptPath)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        string fullPath = Path.Combine(baseDir, scriptPath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"تعذر العثور على الملف. البرنامج يبحث في المسار التالي:\n{fullPath}");
        }

        using var connection = CreateConnection(_databaseName);
        connection.Open();

        string sqlScript = File.ReadAllText(fullPath);

        using var command = new SqlCommand(sqlScript, connection);
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



    

    public static void InitializeDatabase()
    {
        CreateDatabaseIfNotExist();
       // Tables 
       ExecuteScript(@"Scripts\Tables\CreatePersonsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateCustomersTable.sql");
       ExecuteScript(@"Scripts\Tables\CreatePhonesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateTypesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateBrandsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateSpecsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateDevicesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreatePhoneListType.sql");
       ExecuteScript(@"Scripts\Tables\CreateDeviceListType.sql");

        //// Stored Procedure 
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetPagedCustomerSummaries.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_UpdateCustomerInfo.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_SearchCustomerPaged.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_SearchCustomerCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerByID.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_CreateCustomer.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerProfile.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Persons\SP_DeletePerson.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_DeletePhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetCustomerPhones.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_AddCustomerPhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_UpdateCustomerPhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_GetCustomerDevices.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_DeleteCustomerDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_CreateDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_UpdateDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_GetAllBrands.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_AddBrand.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_GetAllSpecs.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_AddSpec.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_GetSpecsByType.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_GetAllTypes.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_AddDeviceType.sql");

        // Functions 
        ExecuteScript(@"Scripts\Functions\GetFirstPersonPhoneNumber.sql");

        // Triggers 
        ExecuteScript(@"Scripts\Triggers\trg_InsteadOfDeletePerson.sql");


    }
}
