Create or Alter procedure SP_IsPhoneExist
@phone VARCHAR(11)
AS
BEGIN
    SET NOCOUNT ON;
    select 1 from Phones Where PhoneNumber = @phone
END