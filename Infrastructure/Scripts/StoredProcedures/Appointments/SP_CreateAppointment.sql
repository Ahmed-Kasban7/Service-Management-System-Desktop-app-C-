CREATE OR ALTER PROCEDURE SP_CreateAppointment 
    @OrderId               INT,
    @TechnicianId          INT,
    @TechnicianAssistantId INT  = NULL, 
    @DriverId              INT  = NULL,
    @ScheduledDate         DATE,
    @VisitType             TINYINT,
    @Notes                 NVARCHAR(MAX) = NULL        
AS
BEGIN
    SET NOCOUNT ON; 
        
    INSERT INTO Appointments (
        OrderId, 
        TechnicianId, 
        TechnicianAssistantId,
        DriverId,
        ScheduledDate, 
        AppointmentState, 
        VisitType, 
        Notes
    )
    VALUES (
        @OrderId, 
        @TechnicianId, 
        @TechnicianAssistantId,
        @DriverId,
        @ScheduledDate, 
        0,
        @VisitType, 
        @Notes
    );        
    
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS AppointmentId;
END