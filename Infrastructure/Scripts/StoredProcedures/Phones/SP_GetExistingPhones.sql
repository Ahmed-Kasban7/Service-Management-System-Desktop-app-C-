CREATE OR ALTER PROCEDURE SP_GetExistingPhones
    @Phones PhoneList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PhoneNumber
    FROM Phones p
    INNER JOIN @Phones temp
        ON p.PhoneNumber = temp.Phone;
END