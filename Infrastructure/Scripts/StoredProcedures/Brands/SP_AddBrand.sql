CREATE OR ALTER PROCEDURE SP_AddBrand
    @BrandName NVARCHAR(max)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = @BrandName)
    BEGIN
            INSERT INTO Brands (BrandName)
            VALUES (@BrandName);
    END

    SELECT @@ROWCOUNT; 


END
