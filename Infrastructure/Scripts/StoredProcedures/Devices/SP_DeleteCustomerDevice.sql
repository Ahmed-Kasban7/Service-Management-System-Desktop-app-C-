CREATE OR ALTER PROCEDURE SP_DeleteCustomerDevice
    @deviceId INT
AS
BEGIN

    DECLARE @customerId INT;

    SELECT @customerId = CustomerID
    FROM Devices
    WHERE DeviceID = @deviceId;

    IF (SELECT COUNT(*) FROM Devices WHERE CustomerID = @customerId) > 1
    BEGIN
        DELETE FROM Devices
        WHERE DeviceID = @deviceId;
    END
END