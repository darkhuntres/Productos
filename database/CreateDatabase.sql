IF DB_ID(N'ProductosDb') IS NULL
BEGIN
    CREATE DATABASE [ProductosDb];
END;
GO

USE [ProductosDb];
GO

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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE TABLE [TiposProducto] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_TiposProducto] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(150) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Rol] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE TABLE [Productos] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(150) NOT NULL,
        [Descripcion] nvarchar(500) NULL,
        [Precio] decimal(18,2) NOT NULL,
        [Stock] int NOT NULL,
        [TipoProductoId] int NOT NULL,
        CONSTRAINT [PK_Productos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Productos_TiposProducto_TipoProductoId] FOREIGN KEY ([TipoProductoId]) REFERENCES [TiposProducto] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[TiposProducto]'))
        SET IDENTITY_INSERT [TiposProducto] ON;
    EXEC(N'INSERT INTO [TiposProducto] ([Id], [Nombre])
    VALUES (1, N''Electrónica''),
    (2, N''Hogar''),
    (3, N''Oficina'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre') AND [object_id] = OBJECT_ID(N'[TiposProducto]'))
        SET IDENTITY_INSERT [TiposProducto] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'PasswordHash', N'Rol') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] ON;
    EXEC(N'INSERT INTO [Usuarios] ([Id], [Email], [PasswordHash], [Rol])
    VALUES (1, N''admin@serfinsa.com'', N''$2a$11$OmBcs9.cT6hz7PpqAiXpcuFPYQwWHmdOH.nQMrEtZuXvUPJr6xbtO'', N''Admin''),
    (2, N''user@serfinsa.com'', N''$2a$11$YyMqjEZzYKEMqf3C6zc.7uxXDs6OEgEZM/eNizxILR2zkWKxcYOZq'', N''User'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'PasswordHash', N'Rol') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE INDEX [IX_Productos_TipoProductoId] ON [Productos] ([TipoProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TiposProducto_Nombre] ON [TiposProducto] ([Nombre]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913014438_InicialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260913014438_InicialCreate', N'10.0.12');
END;

COMMIT;
GO

