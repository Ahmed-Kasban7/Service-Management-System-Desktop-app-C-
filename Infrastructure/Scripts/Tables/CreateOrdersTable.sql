        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
        BEGIN
            CREATE TABLE Orders(
                OrderID INT IDENTITY(1,1) PRIMARY KEY,
                OrderNumber AS ('ORD-' + CAST(OrderID AS VARCHAR)) PERSISTED,
                StartDate DATETIME NOT NULL ,
                EndedDate DATETIME NULL ,
                Problem nvarchar(max) NOT null ,
                Notes nvarchar(Max) NULL,
                CustomerID int  NOT NULL  FOREIGN KEY references Customers(CustomerID) , 
                DeviceID int NOT NULL FOREIGN KEY references Devices(DeviceID),
                OrderState TINYINT NOT NULL CHECK (OrderState >= 0 AND OrderState <= 4)  -- 0 = Pending  , 1 = schedule ,  2 = InProgress , 3 = Completed , 4 = Cancelled
            )
        END