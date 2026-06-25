CREATE OR ALTER PROCEDURE SP_UpdateBrand 
    @brandId INT,
    @brandName nvarchar(200)
AS
BEGIN 
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Brands WHERE BrandName = @brandName AND BrandID <> @brandId)
    BEGIN
        RETURN -1; 
    END

    UPDATE Brands 
    SET BrandName = @brandName
    WHERE BrandID = @brandId;
    
    IF @@ROWCOUNT > 0
        RETURN 1; 
    ELSE
        RETURN 0;

END