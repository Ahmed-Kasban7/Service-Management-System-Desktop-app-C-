CREATE OR Alter TRIGGER trg_AfterCreateAppointment
ON Appointments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON; 

    UPDATE O
    SET O.OrderState = 1 -- 1 = Scheduled
    FROM Orders O
    INNER JOIN INSERTED I ON O.OrderID = I.OrderId
    WHERE O.OrderState = 0; -- 0 = Pending
END