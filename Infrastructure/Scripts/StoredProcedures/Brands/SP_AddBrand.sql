create or alter Procedure SP_AddBrand @brandName nvarchar(100)
as

begin 

insert into Brands (BrandName)
values (@brandName)
end 