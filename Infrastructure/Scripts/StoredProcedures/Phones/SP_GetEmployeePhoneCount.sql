CREATE OR ALTER PROCEDURE SP_GetEmployeePhoneCount
    @employeeId INT,
    @phoneCount INT OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @phoneCount = COUNT(P.PhoneNumber)
    FROM Phones P
    INNER JOIN Employees E ON P.PersonID = E.PersonID
    WHERE E.EmployeeID = @employeeId; 
END;
