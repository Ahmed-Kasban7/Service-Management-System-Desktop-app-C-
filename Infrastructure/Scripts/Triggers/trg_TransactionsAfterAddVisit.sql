CREATE OR ALTER TRIGGER trg_TransactionsAfterAddVisit
ON Visits
AFTER INSERT
AS 
BEGIN 
    SET NOCOUNT ON; 

    --  فى حاله ان مفيش سائق الفلوس تروح للفنى 
   INSERT INTO FinancialTransactions (TransactionType, Amount, EmployeeID, OrderID, VisitID, Status)    
   Select 
        0 , 
        I.TransportationCost , 
        A.TechnicianID,
        A.OrderID,
        I.VisitID,
        0
    FROM Inserted I
    INNER JOIN Appointments A On I.AppointmentID = A.AppointmentID
    WHERE A.DriverId IS NULL and I.TransportationCost>0

    --   فى حاله ان فى سائق  تروح للسائق و السائق بيتحاسب بالمشوار 
    INSERT INTO FinancialTransactions (TransactionType, Amount, EmployeeID, OrderID, VisitID, Status)    
   Select 
        0 , 
        I.TransportationCost , 
        A.DriverId,
        A.OrderID,
        I.VisitID,
        0
    FROM Inserted I
    INNER JOIN Appointments A On I.AppointmentID = A.AppointmentID
    INNER JOIN Employees E ON A.DriverId = E.EmployeeID
    WHERE A.DriverId IS NOT NULL and I.TransportationCost>0 AND E.IsCommissionBased = 1

    -- فى حاله ان فى قطع غيار و فى تكليف انتقالات ليها و فى موظف هو الى راح جابها 

       INSERT INTO FinancialTransactions (TransactionType, Amount, EmployeeID, OrderID, VisitID, Status)    
   Select 
        0 , 
        I.PartsTransportationCost , 
        I.PaidByEmployeeID,
        A.OrderID,
        I.VisitID,
        0
    FROM Inserted I
    INNER JOIN Appointments A On I.AppointmentID = A.AppointmentID
    WHERE I.TransportationBearer = 1  and  I.PartsTransportationCost>0
    




END