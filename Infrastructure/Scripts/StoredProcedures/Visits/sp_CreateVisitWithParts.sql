CREATE OR ALTER PROCEDURE sp_CreateVisitWithParts
    @AppointmentID INT,
    @Notes NVARCHAR(MAX),
    @ActionsTaken NVARCHAR(MAX),
    @Diagnosis NVARCHAR(MAX),
    
    @TotalCostToCustomer DECIMAL(18,2), -- التكلفة على العميل
    @TransportationCost DECIMAL(18,2),   -- تكلفة الانتقالات
    @AmountPaid DECIMAL(18,2),           -- المبلغ المدفوع

    @TransportationBearer TINYINT NULL,           
    @PartsTransportationCost DECIMAL(18,2) NULL,
    @PaidByEmployeeID INT NULL,

    @SparePartsList UsedSparePartsType READONLY 
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Record Visit Report 
          INSERT INTO Visits
        (
            AppointmentID,
            Notes,
            ActionsTaken,
            Diagnosis,
            TotalCostToCustomer,
            TransportationCost,
            AmountPaid,
            TransportationBearer,
            PartsTransportationCost,
            PaidByEmployeeID
        )
          VALUES
        (
            @AppointmentID,
            @Notes,
            @ActionsTaken,
            @Diagnosis,
            @TotalCostToCustomer,
            @TransportationCost,
            @AmountPaid,
            @TransportationBearer,
            @PartsTransportationCost,
            @PaidByEmployeeID
        );

        DECLARE @NewVisitID INT = SCOPE_IDENTITY();

        -- if spareParts Exists add them 
        IF EXISTS (SELECT 1 FROM @SparePartsList)
        BEGIN
            INSERT INTO UsedSpareParts (VisitID, PartName, Quantity, UnitPrice)
            SELECT 
                @NewVisitID, 
                L.PartName, 
                L.Quantity, 
                L.UnitPrice
            FROM @SparePartsList L;
        END

        

        COMMIT TRANSACTION;

        SELECT @NewVisitID;

    END TRY
    BEGIN CATCH

            IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

            THROW;

    END CATCH
END