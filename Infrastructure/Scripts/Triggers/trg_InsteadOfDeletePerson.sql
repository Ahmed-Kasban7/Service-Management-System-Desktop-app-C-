create or alter trigger trg_InsteadOfDeletePerson on Persons
instead of Delete 
as 
begin
update Persons
set IsDeleted = 1
from Persons join deleted on Persons.PersonID = deleted.PersonID
end