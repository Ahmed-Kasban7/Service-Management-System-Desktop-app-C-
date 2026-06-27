CREATE OR ALTER PROCEDURE SP_AddEmployeePhone
    @phoneNumber VARCHAR(11),
    @employeeId INT
AS
BEGIN
     SET NOCOUNT ON;

    Declare @personId INT 

    select @personId = PersonID from Employees where EmployeeID = @employeeId

    INSERT INTO Phones (PhoneNumber, PersonID)
    VALUES (@phoneNumber, @personId);
END