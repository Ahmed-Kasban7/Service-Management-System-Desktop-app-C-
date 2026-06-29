CREATE OR ALTER PROCEDURE SP_GetCompanySettings

AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 CompanyName, CompanyLogo FROM CompanySettings
END;