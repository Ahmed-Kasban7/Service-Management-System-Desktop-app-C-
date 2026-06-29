CREATE OR ALTER PROCEDURE SP_DeleteBrand 
    @brandId INT
AS
BEGIN 
    SET NOCOUNT ON;

    DELETE FROM Brands 
    WHERE BrandID = @brandId
      AND NOT EXISTS (SELECT 1 FROM Devices WHERE BrandID = @brandId);

    SELECT @@ROWCOUNT; 
END