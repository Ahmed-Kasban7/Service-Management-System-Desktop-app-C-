CREATE OR ALTER PROCEDURE SP_DeleteBrand 
    @brandId INT
AS
BEGIN 
    SET NOCOUNT ON;

    DELETE FROM Brands 
    WHERE BrandID = @brandId
      AND NOT EXISTS (SELECT 1 FROM Devices WHERE BrandID = @brandId);

    IF @@ROWCOUNT > 0
        RETURN 1; 
    ELSE
        RETURN 0; 
END