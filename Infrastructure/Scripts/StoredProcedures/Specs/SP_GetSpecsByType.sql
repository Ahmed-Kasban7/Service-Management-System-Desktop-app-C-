
create or alter procedure SP_GetSpecsByType @typeId int 
as 
begin
SELECT 
        s.SpecID, 
        s.SpecName, 
        s.TypeID, 
        t.TypeName
    FROM Specs s
    LEFT JOIN Types t ON s.TypeID = t.TypeID  where s.TypeID = @typeId

end
