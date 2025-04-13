-- Create UserAccounts table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserAccounts' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[UserAccounts]
    (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [FullName] NVARCHAR(100) NULL,
        [UserName] NVARCHAR(50) NOT NULL,
        [Password] NVARCHAR(MAX) NOT NULL,
        [Role] NVARCHAR(50) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_UserAccounts] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_UserAccounts_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
    );
END
GO

-- Create index on UserName for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_UserAccounts_UserName' AND object_id = OBJECT_ID('dbo.UserAccounts'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_UserAccounts_UserName] ON [dbo].[UserAccounts]
    (
        [UserName] ASC
    );
END
GO

-- Create trigger to update the UpdatedAt timestamp
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_UserAccounts_UpdateTimestamp' AND parent_id = OBJECT_ID('dbo.UserAccounts'))
BEGIN
    EXEC('
    CREATE TRIGGER [dbo].[TR_UserAccounts_UpdateTimestamp]
    ON [dbo].[UserAccounts]
    AFTER UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;
        UPDATE [dbo].[UserAccounts]
        SET [UpdatedAt] = GETUTCDATE()
        FROM [dbo].[UserAccounts] u
        INNER JOIN inserted i ON u.Id = i.Id;
    END
    ');
END
GO

