CREATE OR ALTER PROCEDURE SP_UpdateEmployee
    @EmployeeId   INT,
    @Name         NVARCHAR(MAX),
    @Age          INT = NULL,
    @Sex          BIT,
    @Address      NVARCHAR(MAX) = NULL,
    @RoleId       INT,
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PersonId INT;

    SELECT @PersonId = PersonID
    FROM Employees
    WHERE EmployeeId = @EmployeeId;

    BEGIN TRANSACTION;

    BEGIN TRY

        UPDATE Persons
        SET
            Name = @Name,
            Age = @Age,
            Sex = @Sex
        WHERE PersonID = @PersonId;

        UPDATE Employees
        SET
            Address = @Address ,
            RoleID = @RoleId,
            DepartmentID = @DepartmentId
        WHERE EmployeeId = @EmployeeId;

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END