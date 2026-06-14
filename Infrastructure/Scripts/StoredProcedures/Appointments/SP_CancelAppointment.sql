CREATE OR ALTER PROCEDURE SP_CancelAppointment
    @AppointmentId INT , 
    @CancelReason  NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

        UPDATE Appointments
        SET AppointmentState = 3 , CancelReason = @CancelReason
        WHERE AppointmentId = @AppointmentId
          AND AppointmentState IN (0, 1);
END;
