create or alter procedure SP_AddDeviceType @TypeName nvarchar(100)
as 
begin 
insert into Types (TypeName)
values (@TypeName)
end 