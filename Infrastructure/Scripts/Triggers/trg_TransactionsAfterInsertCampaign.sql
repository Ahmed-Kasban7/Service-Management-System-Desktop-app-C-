CREATE OR ALTER TRIGGER trg_TransactionsAfterInsertCampaign
ON Campaigns
AFTER INSERT
AS 
BEGIN 
    SET NOCOUNT ON; 

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @CampaignID INT;
        DECLARE @CampaignName NVARCHAR(100);
        DECLARE @CampaignCost DECIMAL(18,2);
        
        DECLARE @balanceBefore DECIMAL(18,2);
        DECLARE @balanceAfter DECIMAL(18,2);
        DECLARE @InsertedTransactionID INT;

        SELECT @CampaignID = CampaignID, 
               @CampaignName = CampaignName, 
               @CampaignCost = CampaignCost 
        FROM INSERTED;

        IF (@CampaignCost > 0)
        BEGIN
            SELECT @balanceBefore = CurrentBalance FROM Treasuries WHERE TreasuryID = 1;
            SET @balanceAfter = @balanceBefore - @CampaignCost;

            INSERT INTO TreasuryTransactions (TreasuryID, Amount, TransactionType, BalanceBefore, BalanceAfter, CategoryID, TransactionDate , Description)
            VALUES (1, @CampaignCost, 0, @balanceBefore, @balanceAfter, 1, GETDATE() , N'مصاريف حملة: ' + @CampaignName);

            DECLARE @CampaignTypeID INT = (SELECT TypeID FROM ReferenceTypes WHERE TypeName = 'Campaign');
            DECLARE @TransactionID INT = SCOPE_IDENTITY();

            INSERT INTO ReferenceTransactions (TransactionID, TypeID)
            VALUES (@TransactionID, @CampaignTypeID);

            UPDATE Treasuries
            SET CurrentBalance = @balanceAfter,
                LastUpdated = GETDATE()
            WHERE TreasuryID = 1;

        END

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

            THROW;
    END CATCH
END
