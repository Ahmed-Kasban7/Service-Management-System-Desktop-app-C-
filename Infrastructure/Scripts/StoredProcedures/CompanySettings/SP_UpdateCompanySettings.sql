CREATE OR ALTER PROCEDURE SP_UpdateCompanySettings
    @CompanyName NVARCHAR(MAX),
 
    @CompanyLogo VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CompanySettings)
    BEGIN
        INSERT INTO CompanySettings (CompanyName, CompanyLogo)
        VALUES (@CompanyName,@CompanyLogo);
    END
    ELSE
    BEGIN
        UPDATE CompanySettings
        SET CompanyName = @CompanyName,
            CompanyLogo = ISNULL(@CompanyLogo, CompanyLogo) 
        WHERE Id = 1; 
    END
END;