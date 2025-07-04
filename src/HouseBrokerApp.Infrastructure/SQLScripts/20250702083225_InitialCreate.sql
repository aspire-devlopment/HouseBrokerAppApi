IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [CommissionRates] (
    [Id] int NOT NULL IDENTITY,
    [MinPrice] decimal(18,2) NULL,
    [MaxPrice] decimal(18,2) NULL,
    [Rate] decimal(5,4) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_CommissionRates] PRIMARY KEY ([Id])
);

CREATE TABLE [PropertyListings] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(150) NOT NULL,
    [Description] nvarchar(max) NULL,
    [PropertyType] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Location] nvarchar(max) NULL,
    [Features] nvarchar(max) NULL,
    [ImageUrl] nvarchar(max) NULL,
    [BrokerId] int NOT NULL,
    [CommissionAmount] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_PropertyListings] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250702083225_InitialCreate', N'9.0.6');

COMMIT;
GO

