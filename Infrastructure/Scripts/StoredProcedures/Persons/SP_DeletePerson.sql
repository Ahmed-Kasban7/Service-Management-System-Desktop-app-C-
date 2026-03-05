create or alter Procedure SP_DeletePerson @personId int 
as 
begin
delete from Persons where personId = @personId;
end