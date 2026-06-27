CREATE OR ALTER PROCEDURE SP_UpdatePhone
    @oldPhone VARCHAR(11),
    @newPhone VARCHAR(11)
AS
BEGIN

    UPDATE Phones
    SET PhoneNumber = @newPhone
    WHERE PhoneNumber = @oldPhone;
END