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

    public static void SeedData()
    {
        using var connection = CreateConnection(_databaseName);
        connection.Open();

        string sql = @"
IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = N'الصيانة')
BEGIN
    INSERT INTO Departments (DepartmentName)
    VALUES (N'الصيانة');
END;

IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = N'النقل')
BEGIN
    INSERT INTO Departments (DepartmentName)
    VALUES (N'النقل');
END;

IF NOT EXISTS (
    SELECT 1
    FROM DepartmentRoles
    WHERE RoleName = N'سائق'
)
BEGIN
    INSERT INTO DepartmentRoles (RoleName, DepartmentID)
    VALUES (
        N'سائق',
        (SELECT DepartmentID
         FROM Departments
         WHERE DepartmentName = N'النقل')
    );
END;

IF NOT EXISTS (
    SELECT 1
    FROM DepartmentRoles
    WHERE RoleName = N'فنى'
)
BEGIN
    INSERT INTO DepartmentRoles (RoleName, DepartmentID)
    VALUES (
        N'فنى',
        (SELECT DepartmentID
         FROM Departments
         WHERE DepartmentName = N'الصيانة')
    );
END;

IF NOT EXISTS (
    SELECT 1
    FROM DepartmentRoles
    WHERE RoleName = N'مساعد فنى'
)
BEGIN
    INSERT INTO DepartmentRoles (RoleName, DepartmentID)
    VALUES (
        N'مساعد فنى',
        (SELECT DepartmentID
         FROM Departments
         WHERE DepartmentName = N'الصيانة')
    );
END;

IF NOT EXISTS (SELECT 1 FROM Treasuries WHERE TreasuryName = N'الخزنة الرئيسية')
BEGIN
    INSERT INTO Treasuries (TreasuryName, CurrentBalance, LastUpdated)
    VALUES (N'الخزنة الرئيسية', 0.00, GETDATE());
END;

IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = N'مصاريف حملات')
BEGIN
    INSERT INTO Categories (CategoryName) VALUES (N'مصاريف حملات');
END;


";

        using var cmd = new SqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }



    public static void InitializeDatabase()
    {
        CreateDatabaseIfNotExist();

       // Tables 
       ExecuteScript(@"Scripts\Tables\CreatePersonsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateSourcesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateCampaignsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateCustomersTable.sql");
       ExecuteScript(@"Scripts\Tables\CreatePhonesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateTypesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateBrandsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateSpecsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateDevicesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateOrdersTable.sql");
       ExecuteScript(@"Scripts\Tables\CreatePhoneListType.sql");
       ExecuteScript(@"Scripts\Tables\CreateDeviceListType.sql");
       ExecuteScript(@"Scripts\Tables\CreateDepartmentsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateDepartmentRolesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateEmployeesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateAppointmentsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateAppointmentsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateVisitsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateUsedSpareParts.sql");
       ExecuteScript(@"Scripts\Tables\CreateUsedSparePartsType.sql");
       ExecuteScript(@"Scripts\Tables\CreateFinancialTransactionsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateTreasuriesTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateCategoryTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateTreasuryTransactionsTable.sql");
       ExecuteScript(@"Scripts\Tables\CreateReferenceTransactionsTable.sql");
       ExecuteScript(@"Scripts\Tables\EmployeeAttachments.sql");
       ExecuteScript(@"Scripts\Tables\AttachmentList.sql");
       ExecuteScript(@"Scripts\Tables\CompanySettings.sql");



        SeedData();



        // Stored Procedure 
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetPagedCustomerSummaries.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_UpdateCustomerInfo.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_SearchCustomerPaged.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerByID.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_CreateCustomer.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomerProfile.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_IsCustomerExist.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_GetCustomersLookup.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Customers\SP_DeleteCustomer.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Persons\SP_DeletePerson.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_DeletePhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetCustomerPhones.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetEmployeePhoneCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_AddCustomerPhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_AddEmployeePhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_UpdatePhone.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetExistingPhones.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_IsPhoneExist.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetEmployeePhones.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Phones\SP_GetCustomerPhoneCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_GetCustomerDevices.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_DeleteCustomerDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_CreateDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_UpdateDevice.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_IsDeviceExist.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_IsDeviceAssignedToCustomer.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Devices\SP_CustomerDeviceLookup.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_GetAllBrands.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_AddBrand.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_DeleteBrand.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Brands\SP_UpdateBrand.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_GetAllSpecs.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_AddSpec.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_DeleteSpec.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_UpdateSpec.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Specs\SP_GetSpecsByType.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_GetAllTypes.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_AddDeviceType.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_DeleteType.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Types\SP_UpdateType.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_CreateOrder.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_UpdateOrder.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_GetOrderById.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_GetPagedOrderSummaries.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_GetOrderFullDetailsById.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_SearchOrderPage.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_GetDeviceOrdersHistory.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Orders\SP_GetCustomerOrders.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_GetTechniciansLookup.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_GetAllEmployeesLookup.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_CreateEmployee.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_SearchEmployee.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_GetPagedEmployeeSummaries.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_GetEmployeeProfile.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Employees\SP_UpdateEmployee.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Appointments\SP_CreateAppointment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Appointments\SP_GetAppointmentsByOrderId.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Appointments\SP_UpdateAppointment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Appointments\SP_GetAppointmentById.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Appointments\SP_CancelAppointment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Visits\sp_CreateVisitWithParts.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Visits\SP_VisitDetails.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Sources\SP_GetAllSources.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Sources\SP_UpdateSource.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Sources\SP_AddSource.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Sources\SP_DeleteSource.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_CreateCampaign.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_GetPagedCampaignSummaries.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_SearchCampaignPaged.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_CampaignDetails.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_DeleteCampaign.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_UpdateCampaign.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_GetCampaignBySourceId.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Campaigns\SP_CampaignCustomerCount.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Treasuries\SP_GetCurrentBalance.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Departments\SP_GetDepartmentsLookup.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Departments\SP_AddDepartment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Departments\SP_UpdateDepartment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\Departments\SP_DeleteDepartment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\DepartmentRoles\SP_GetRolesByDepartment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\DepartmentRoles\SP_AddDepartmentRole.sql");
        ExecuteScript(@"Scripts\StoredProcedures\DepartmentRoles\SP_DeleteRole.sql");
        ExecuteScript(@"Scripts\StoredProcedures\DepartmentRoles\SP_UpdateRole.sql");
        ExecuteScript(@"Scripts\StoredProcedures\DepartmentRoles\SP_GetAllDepartmentRoles.sql");
        ExecuteScript(@"Scripts\StoredProcedures\EmployeeAttachments\SP_GetEmployeeAttachments.sql");
        ExecuteScript(@"Scripts\StoredProcedures\EmployeeAttachments\SP_AddAttachment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\EmployeeAttachments\SP_DeleteEmployeeAttachment.sql");
        ExecuteScript(@"Scripts\StoredProcedures\CompanySettings\SP_UpdateCompanySettings.sql");
        ExecuteScript(@"Scripts\StoredProcedures\CompanySettings\SP_GetCompanySettings.sql");
        ExecuteScript(@"Scripts\StoredProcedures\CompanySettings\SP_Backup.sql");



        // Functions 
        ExecuteScript(@"Scripts\Functions\GetFirstPersonPhoneNumber.sql");

        // Triggers 
        //ExecuteScript(@"Scripts\Triggers\trg_InsteadOfDeletePerson.sql");
        ExecuteScript(@"Scripts\Triggers\trg_AfterCreateAppointment.sql");
        ExecuteScript(@"Scripts\Triggers\trg_AfterCancelAppointment.sql");
        ExecuteScript(@"Scripts\Triggers\trg_UpdateStatuesAfterAddVisit.sql");
       // ExecuteScript(@"Scripts\Triggers\trg_TransactionsAfterAddVisit.sql");
        ExecuteScript(@"Scripts\Triggers\trg_TransactionsAfterInsertCampaign.sql");


    }
}
