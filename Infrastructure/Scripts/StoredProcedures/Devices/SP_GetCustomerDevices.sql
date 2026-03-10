CREATE OR ALTER PROCEDURE SP_GetCustomerDevices
    @customerId INT
AS
BEGIN

    SELECT 
        d.DeviceID,
        d.BrandID,  
        d.TypeID,   
        d.SpecID,   
        b.BrandName AS Brand, 
        t.TypeName AS Type, 
        s.SpecName AS Spec, 
        d.ModelName, 
        d.SerialNumber
    FROM Devices d
    LEFT JOIN Brands b ON d.BrandID = b.BrandID
    LEFT JOIN Types t ON d.TypeID = t.TypeID
    LEFT JOIN Specs s ON d.SpecID = s.SpecID
    WHERE d.CustomerID = @customerId;
END