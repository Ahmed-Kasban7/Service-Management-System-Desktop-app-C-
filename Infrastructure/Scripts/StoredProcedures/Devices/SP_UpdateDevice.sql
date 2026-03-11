CREATE OR ALTER PROCEDURE SP_UpdateDevice
    @deviceId INT,
    @brandId INT,
    @typeId INT,
    @specId INT,
    @model NVARCHAR(200),
    @serial NVARCHAR(100)
AS
BEGIN

    UPDATE Devices 
    SET BrandID = @brandId,
        TypeID = @typeId,
        SpecID = @specId,
        ModelName = @model,
        SerialNumber = @serial
    WHERE DeviceID = @deviceId;
END