CREATE OR ALTER PROCEDURE SP_IsDeviceExist
    @deviceId INT
AS
BEGIN

    SELECT 
    1
    FROM Devices 
    WHERE  DeviceID = @deviceId;
END