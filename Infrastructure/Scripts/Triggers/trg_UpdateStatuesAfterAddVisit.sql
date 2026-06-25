CREATE OR ALTER TRIGGER trg_UpdateStatusAfterAddVisit
ON Visits
AFTER INSERT
AS 
BEGIN 
    SET NOCOUNT ON; 

    UPDATE Appointments
    SET AppointmentState = 2 -- complete
    WHERE AppointmentId IN (SELECT AppointmentID FROM Inserted);

    Update Orders
    set OrderState = 2
    where OrderID IN (
         select A.OrderID 
         From Appointments A 
         Inner Join Inserted I On A.AppointmentId = I.AppointmentID
    ) and OrderState = 1 

END