create or alter Function GetFirstPersonPhoneNumber(@personId int )
returns varchar(11)
as 
begin
declare @PhNumber varchar(11)  
select top 1  @PhNumber = PhoneNumber from Phones where personid = @personId
return @PhNumber
end
