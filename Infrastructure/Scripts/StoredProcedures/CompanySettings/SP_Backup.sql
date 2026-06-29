CREATE OR ALTER PROCEDURE SP_Backup 
    @databaseName NVARCHAR(MAX),
    @backupDirectory NVARCHAR(MAX) = N'D:\Database_Backup\'
AS 
BEGIN 
    SET NOCOUNT ON;

    DECLARE @sqlCommand NVARCHAR(MAX);
    DECLARE @fileName NVARCHAR(MAX);
    DECLARE @fullBackupPath NVARCHAR(MAX);

    SET @fileName = @databaseName + '_Full_Current.bak';
    SET @fullBackupPath = @backupDirectory + @fileName;

    SET @sqlCommand = N'BACKUP DATABASE [' + @databaseName + N'] 
                        TO DISK = N''' + @fullBackupPath + N''' 
                        WITH FORMAT, 
                        NAME = N''Current Full Backup of ' + @databaseName + N''';';

    BEGIN TRY
        EXEC sp_executesql @sqlCommand;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;