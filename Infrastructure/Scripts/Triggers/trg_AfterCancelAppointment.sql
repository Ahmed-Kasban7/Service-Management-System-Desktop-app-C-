CREATE OR ALTER TRIGGER trg_AfterCancelAppointment
ON Appointments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(AppointmentState)
    BEGIN
        UPDATE O
        SET O.OrderState = 0
        FROM Orders O
        INNER JOIN inserted i ON i.OrderId = O.OrderID
        WHERE i.AppointmentState = 3
          AND O.OrderState = 1
          AND NOT EXISTS (
                SELECT 1
                FROM Appointments a
                WHERE a.OrderId = O.OrderID
                  AND a.AppointmentState IN (0, 1)
                  AND a.AppointmentId <> i.AppointmentId
            );
    END
END;