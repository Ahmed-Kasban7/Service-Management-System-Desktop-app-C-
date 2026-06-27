CREATE OR ALTER PROCEDURE SP_CreateEmployee
    @Name              NVARCHAR(200),
    @Age               INT = NULL,
    @Sex               TINYINT,
    @Address           NVARCHAR(max) = NULL,
    @HireDate          DATE,
    @RoleId            INT,
    @DepartmentId      INT,
    @CompensationType TINYINT , 
    @BaseSalary        DECIMAL(10,2) = NULL,
    @CommissionPercent DECIMAL(5,2)  = NULL,
    @Phones            PhoneList READONLY,
    @Attachments       AttachmentList READONLY,
    @EmployeeId        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @PersonId INT;

        -- 1. Create Person
        INSERT INTO Persons (Name, Age, Sex)
        VALUES (@Name, @Age, @Sex);
        SET @PersonId = SCOPE_IDENTITY();

        -- 2. Create Employee
        INSERT INTO Employees (PersonID, HireDate, RoleID, DepartmentID,CompensationType, BaseSalary, CommissionPercent , Address)
        VALUES (@PersonId, @HireDate, @RoleId, @DepartmentId,@CompensationType, @BaseSalary, @CommissionPercent , @Address);
        SET @EmployeeId = SCOPE_IDENTITY();

        -- 3. Add Phones
        INSERT INTO Phones (PersonID, PhoneNumber)
        SELECT @PersonId, Phone
        FROM @Phones;

        -- 4. Add Attachments
        INSERT INTO EmployeeAttachments (EmployeeId, FilePath)
        SELECT @EmployeeId, FilePath
        FROM @Attachments;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END