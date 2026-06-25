IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'Campaigns'
)
BEGIN
    CREATE TABLE Campaigns
    (
        CampaignId INT IDENTITY(1,1) PRIMARY KEY,

        CampaignName NVARCHAR(200) NOT NULL,

        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,

        Notes NVARCHAR(MAX) NULL,

        IsActive BIT NOT NULL
            CONSTRAINT DF_Campaigns_IsActive DEFAULT (1),

        SourceID INT NOT NULL,

        Discount INT NOT NULL
            CONSTRAINT DF_Campaigns_Discount DEFAULT (0),

        CampaignCost DECIMAL(18,2) NOT NULL,

        CONSTRAINT CK_Campaigns_Dates
            CHECK (EndDate >= StartDate),

        CONSTRAINT CK_Campaigns_Discount
            CHECK (Discount BETWEEN 0 AND 100),

        CONSTRAINT CK_Campaigns_Cost
            CHECK (CampaignCost >= 0),

        CONSTRAINT FK_Campaigns_Source
            FOREIGN KEY (SourceID)
            REFERENCES Sources(SourceID)
    );
END