CREATE OR ALTER PROCEDURE SP_UpdateAppointment 
    @AppointmentId          INT, 
    @TechnicianId           INT, 
    @TechnicianAssistantId  INT = NULL, 
    @DriverId               INT = NULL,
    @ScheduledDate          DATETIME, 
    @VisitType              TINYINT, 
    @Notes                  NVARCHAR(MAX) = NULL 
AS 
BEGIN 
    SET NOCOUNT ON;

    UPDATE Appointments 
    SET 
        TechnicianId          = @TechnicianId, 
        TechnicianAssistantId = @TechnicianAssistantId, 
        DriverId              = @DriverId,
          AppointmentState = CASE
            WHEN AppointmentState = 1 AND CAST(@ScheduledDate AS DATE) >= CAST(GETDATE() AS DATE)
            THEN 0
            ELSE AppointmentState
        END , 
        ScheduledDate         = @ScheduledDate, 
        VisitType             = @VisitType, 
        Notes                 = @Notes
    WHERE AppointmentId = @AppointmentId
      AND AppointmentState IN (0, 1);
END