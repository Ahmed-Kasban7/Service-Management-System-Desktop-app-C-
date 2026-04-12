create or alter procedure SP_AddSpec @spec nvarchar(100) , @typeId int
as 
begin 
insert into Specs (SpecName , TypeID)
values (@spec , @typeId)
end 