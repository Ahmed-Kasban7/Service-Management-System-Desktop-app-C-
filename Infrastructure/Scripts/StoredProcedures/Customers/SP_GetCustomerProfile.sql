CREATE OR ALTER PROCEDURE SP_GetCustomerProfile
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.CustomerNumber,
        c.CustomerID,
        p.Name,
        p.Sex,
        p.Age,
        c.Address,
        c.Discount, 
        p.DateCreated,
        
        s.SourceName,
        cam.CampaignName
    FROM Customers c
    INNER JOIN Persons p ON p.PersonID = c.PersonID
    LEFT JOIN Sources s ON s.SourceID = c.SourceID        
    LEFT JOIN Campaigns cam ON cam.CampaignID = c.CampaignID  
    WHERE c.CustomerID = @id AND p.IsActive = 1

END
