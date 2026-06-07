CREATE OR ALTER PROCEDURE SP_CustomerDeviceLookup
    @customerId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        d.DeviceID,
        CONCAT(
            d.DeviceID , '#', ' - ',
            ISNULL(t.TypeName, ''), ' ',
            ISNULL(b.BrandName, ''), ' ',
            ISNULL(s.SpecName, ''), ' ',
            ISNULL(d.ModelName, ''), ' ',
            CASE 
                WHEN d.SerialNumber IS NOT NULL 
                THEN CONCAT('(S/N: ', d.SerialNumber, ')')
                ELSE ''
            END
        ) AS DeviceFullName
    FROM Devices d
    LEFT JOIN Brands b ON d.BrandID = b.BrandID
    LEFT JOIN Types t ON d.TypeID = t.TypeID
    LEFT JOIN Specs s ON d.SpecID = s.SpecID
    WHERE d.CustomerID = @customerId
    ORDER BY d.DeviceID;
END