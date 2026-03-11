CREATE OR ALTER PROCEDURE SP_CreateDevice
    @customerId INT,
    @brandId INT,
    @typeId INT,
    @specId INT,
    @serialNumber NVARCHAR(100),
    @modelName NVARCHAR(200)
AS
BEGIN

    INSERT INTO Devices
    (
        CustomerID,
        BrandID,
        TypeID,
        SpecID,
        SerialNumber,
        ModelName
    )
    VALUES
    (
        @customerId,
        @brandId,
        @typeId,
        @specId,
        @serialNumber,
        @modelName
    );
END