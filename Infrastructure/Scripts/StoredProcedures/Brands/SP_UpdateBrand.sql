CREATE OR ALTER PROCEDURE SP_UpdateBrand 
    @brandId INT,
    @brandName nvarchar(200)
AS
BEGIN 
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = @brandName AND BrandID <> @brandId)
    BEGIN
        UPDATE Brands 
        SET BrandName = @brandName
        WHERE BrandID = @brandId;
    END

    
    SELECT @@ROWCOUNT; 

END