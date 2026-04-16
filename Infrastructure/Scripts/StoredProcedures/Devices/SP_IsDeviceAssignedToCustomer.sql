CREATE OR ALTER PROCEDURE SP_IsDeviceAssignedToCustomer
    @deviceId INT,
    @customerId INT
AS
BEGIN

    IF EXISTS (
    SELECT 1
    FROM Devices
    WHERE DeviceID = @deviceId
      AND CustomerID = @customerId
)
BEGIN
    SELECT 1
END
ELSE
BEGIN 
    SELECT 0
END

END