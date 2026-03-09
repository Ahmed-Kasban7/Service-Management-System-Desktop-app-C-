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
        using var connection = CreateConnection(_databaseName);
        connection.Open();

        string sqlScript = File.ReadAllText(scriptPath);

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
       ExecuteScript(@"Scripts\Tables\CreatePhoneNumberIndex.sql");
       ExecuteScript(@"Scripts\Tables\CreateTypesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateBrandsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateSpecsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateDevicesTable.sql");

       // Stored Procedure 
       ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetPagedCustomerSummaries.sql");
       ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_UpdateCustomerInfo.sql");
       ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_SearchCustomerPaged.sql");
       ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_SearchCustomerCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerByID.sql");
       ExecuteScript(@"Scripts\StoredProcedures\Persons\SP_DeletePerson.sql");

       // Functions 
       ExecuteScript(@"Scripts\Functions\GetFirstPersonPhoneNumber.sql");

        // Triggers 
       ExecuteScript(@"Scripts\Triggers\trg_InsteadOfDeletePerson.sql");

        
    }
}
