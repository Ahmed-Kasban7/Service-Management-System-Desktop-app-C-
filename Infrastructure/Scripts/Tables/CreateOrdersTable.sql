        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
        BEGIN
            CREATE TABLE Orders(
                OrderID INT IDENTITY(1,1) PRIMARY KEY,
              OrderNumber AS (
   'ORD-' + RIGHT('000000' + CAST(OrderId AS VARCHAR(10)), 6)
) PERSISTED ,
                StartDate DATETIME NOT NULL DEFAULT GETDATE() ,
                EndedDate DATETIME NULL ,
                Problem nvarchar(max) NOT null ,
                Notes nvarchar(Max) NULL,
                CustomerID int  NOT NULL  , 
                DeviceID int NOT NULL ,
                OrderState TINYINT NOT NULL CHECK  (OrderState BETWEEN 0 AND 4) ,  -- 0 = Pending  , 1 = schedule ,  2 = InProgress , 3 = Completed , 4 = Cancelled
                CONSTRAINT FK_Orders_Customers
                    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),

                CONSTRAINT FK_Orders_Devices
                FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
            )
        END