CREATE OR ALTER PROCEDURE SP_CreateCustomer
    @Name NVARCHAR(200),
    @Age INT,
    @Sex TINYINT,
    @Discount INT,
    @Address NVARCHAR(500),
    @Phones PhoneList READONLY,
    @Devices DeviceList READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @personId INT;
        DECLARE @customerId INT;

        -- 1. Create Person
        INSERT INTO Persons (Name, Age, Sex)
        VALUES (@Name, @Age, @Sex);

        SET @personId = SCOPE_IDENTITY();

        -- 2. Create Customer
        INSERT INTO Customers (PersonID, Address, Discount)
        VALUES (@personId, @Address, @Discount);

        SET @customerId = SCOPE_IDENTITY();

        -- 3. Add Phones
        INSERT INTO Phones (PersonID, PhoneNumber)
        SELECT @personId, Phone
        FROM @Phones;

        -- 4. Add Devices
        INSERT INTO Devices (CustomerID, BrandID, TypeID, SpecID, SerialNumber, ModelName)
        SELECT @customerId, BrandId, TypeId, SpecId, SerialNumber, Model
        FROM @Devices;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END