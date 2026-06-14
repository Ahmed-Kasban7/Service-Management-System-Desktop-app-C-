CREATE OR ALTER PROCEDURE sp_CreateVisitWithParts
    @AppointmentID INT,
    @Notes NVARCHAR(MAX),
    @ActionsTaken NVARCHAR(MAX),
    @Diagnosis NVARCHAR(MAX),
    @TotalCost DECIMAL(18,2),
    @SparePartsList UsedSparePartsType READONLY 
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Visits (AppointmentID, Notes, ActionsTaken, Diagnosis, TotalCost)
        VALUES (@AppointmentID, @Notes, @ActionsTaken, @Diagnosis, @TotalCost);

        DECLARE @NewVisitID INT = SCOPE_IDENTITY();

        IF EXISTS (SELECT 1 FROM @SparePartsList)
        BEGIN
            INSERT INTO SparePartsNames (PartName)
            SELECT DISTINCT L.PartName
            FROM @SparePartsList L
            WHERE NOT EXISTS (
                SELECT 1 FROM SparePartsNames S WHERE S.PartName = L.PartName
            );

            INSERT INTO UsedSpareParts (VisitID, PartId, Quantity, UnitPrice)
            SELECT 
                @NewVisitID, 
                S.PartId, 
                L.Quantity, 
                L.UnitPrice
            FROM @SparePartsList L
            INNER JOIN SparePartsNames S ON S.PartName = L.PartName;
        END

        UPDATE Appointments
        SET AppointmentState = 2 
        WHERE AppointmentId = @AppointmentID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
