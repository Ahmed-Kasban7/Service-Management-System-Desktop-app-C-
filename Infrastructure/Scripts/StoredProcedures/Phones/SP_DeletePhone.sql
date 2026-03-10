create or alter procedure SP_DeletePhone @PhoneNumber varchar(11)
as 
begin 
DELETE FROM Phones WHERE Phones.PhoneNumber = @PhoneNumber
end