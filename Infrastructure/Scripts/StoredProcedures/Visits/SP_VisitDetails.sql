CREATE OR ALTER PROCEDURE SP_VisitDetails
    @AppointmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.ScheduledDate,
        A.VisitType,
        P_Tech.Name AS TechnicianName,
        P_Assistant.Name AS AssistantName,
        P_Driver.Name AS DriverName,    
        P_PaidBy.Name AS PaidByEmployeeName,
        

        V.Diagnosis,
        V.ActionsTaken,
        V.Notes,

        V.TotalCostToCustomer,
        V.TransportationCost,
        V.AmountPaid,
        V.RemainingAmount, 

        V.PartsTransportationCost,
        V.TransportationBearer
        
    FROM Visits V
    INNER JOIN Appointments A ON V.AppointmentID = A.AppointmentID
    
    INNER JOIN Employees E_Tech ON A.TechnicianID = E_Tech.EmployeeID
    LEFT JOIN Persons P_Tech ON E_Tech.PersonID = P_Tech.PersonID

    LEFT JOIN Employees E_Driver ON A.DriverId = E_Driver.EmployeeID 
    LEFT JOIN Persons P_Driver ON E_Driver.PersonID = P_Driver.PersonID

    LEFT JOIN Employees E_Assistant ON A.TechnicianAssistantId = E_Assistant.EmployeeID 
    LEFT JOIN Persons P_Assistant ON E_Assistant.PersonID = P_Assistant.PersonID

    LEFT JOIN Employees E_PaidBy ON V.PaidByEmployeeID = E_PaidBy.EmployeeID 
    LEFT JOIN Persons P_PaidBy ON E_PaidBy.PersonID = P_PaidBy.PersonID
    

    WHERE V.AppointmentID = @AppointmentID;


    SELECT 
        PartName,
        Quantity,
        UnitPrice,
        TotalPrice
    FROM UsedSpareParts
    WHERE VisitID = (SELECT VisitID FROM Visits WHERE AppointmentID = @AppointmentID);


END