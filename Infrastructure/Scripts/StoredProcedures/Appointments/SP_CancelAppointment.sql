CREATE OR ALTER PROCEDURE SP_CancelAppointment
    @AppointmentId INT
AS
BEGIN
    SET NOCOUNT ON;

        UPDATE Appointments
        SET AppointmentState = 3
        WHERE AppointmentId = @AppointmentId
          AND AppointmentState IN (0, 1);
END;
