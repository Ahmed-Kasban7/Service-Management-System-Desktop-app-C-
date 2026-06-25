Create OR Alter procedure SP_GetAllSources
as

begin
select  SourceID , SourceName from Sources ;

end