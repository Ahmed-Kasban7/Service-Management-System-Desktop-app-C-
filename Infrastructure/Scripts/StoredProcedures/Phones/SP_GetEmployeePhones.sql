CREATE OR ALTER PROCEDURE SP_GetEmployeePhones
    @employeeId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.PhoneNumber
    FROM Phones P
    INNER JOIN Employees e ON P.PersonID = e.PersonID
    WHERE e.EmployeeID = @employeeId;

END;