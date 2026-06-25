CREATE OR ALTER PROCEDURE SP_AddBrand
    @BrandName NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Brands WHERE TRIM(BrandName) = TRIM(@BrandName))
    BEGIN
        SELECT 0 AS Result;
        RETURN;
    END

    INSERT INTO Brands (BrandName)
    VALUES (TRIM(@BrandName));

    SELECT 1 AS Result;
END
