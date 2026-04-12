CREATE OR ALTER PROCEDURE SP_CreateCustomer
@Name nvarchar(200),
@Age int,
@Sex TINYINT,
@Discount int,
@Address nvarchar(500),
@Phones PhoneList READONLY,
@Devices DeviceList READONLY
AS
BEGIN
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @personId INT;

    -- create person
    INSERT INTO Persons(Name , Age , Sex)
    VALUES(@Name , @Age , @Sex);

    
    SET @personId = SCOPE_IDENTITY();

    -- create customer
    INSERT INTO Customers(PersonID, Address , Discount)
    VALUES (@personId, @Address , @Discount);


    -- add phones
    INSERT INTO Phones(PersonID , PhoneNumber)
    SELECT @personId , Phone
    FROM @Phones;

    -- add devices
    INSERT INTO Devices
    (CustomerID , BrandID , TypeID , SpecID , SerialNumber , ModelName)
    SELECT
    @CustomerId ,
    BrandId ,
    TypeId ,
    SpecId ,
    SerialNumber ,
    Model
    FROM @Devices;

    COMMIT TRANSACTION;


END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION;

    THROW;

END CATCH

END