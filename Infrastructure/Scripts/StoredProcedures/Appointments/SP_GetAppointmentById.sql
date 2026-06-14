CREATE OR ALTER PROCEDURE SP_GetAppointmentById
    @AppointmentId INT
AS
BEGIN
    SELECT 
        a.AppointmentId,
        a.OrderId,
        a.ScheduledDate,
        a.VisitType,
        a.AppointmentState,
        a.Notes,
        a.TechnicianId,
        a.TechnicianAssistantId , 
        a.DriverId
    FROM Appointments a
    WHERE a.AppointmentId = @AppointmentId
END