CREATE OR ALTER PROCEDURE SP_GetAllSpecs  
AS  
BEGIN  
    SELECT 
        s.SpecID, 
        s.SpecName, 
        s.TypeID, 
        t.TypeName
    FROM Specs s
    LEFT JOIN Types t ON s.TypeID = t.TypeID 
END