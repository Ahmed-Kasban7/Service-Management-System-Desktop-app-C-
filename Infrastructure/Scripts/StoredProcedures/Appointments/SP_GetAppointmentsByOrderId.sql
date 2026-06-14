CREATE OR ALTER PROCEDURE SP_GetAppointmentsByOrderId
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON; 
    SELECT 
        a.AppointmentId,
        a.ScheduledDate,
        a.VisitType,
        a.AppointmentState,
        a.Notes,
        pt.Name   AS TechnicianName,
        pa.Name   AS AssistantName,
        pd.Name   AS DriverName 
        
    FROM Appointments a
    INNER JOIN Employees et ON a.TechnicianId = et.EmployeeId
    INNER JOIN Persons   pt ON et.PersonID   = pt.PersonID
    
    LEFT JOIN Employees ea  ON a.TechnicianAssistantId = ea.EmployeeId
    LEFT JOIN Persons   pa  ON ea.PersonID = pa.PersonID
    
    LEFT JOIN Employees ed  ON a.DriverId = ed.EmployeeId
    LEFT JOIN Persons   pd  ON ed.PersonID = pd.PersonID
    
    WHERE a.OrderId = @OrderId
    ORDER BY a.ScheduledDate DESC;
END
