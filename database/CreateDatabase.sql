IF DB_ID('ProductosDb') IS NULL
BEGIN
    CREATE DATABASE ProductosDb;
END
GO

USE ProductosDb;
GO


-- Tipos de producto
IF OBJECT_ID('TiposProducto', 'U') IS NULL
BEGIN
    CREATE TABLE TiposProducto
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO


-- Usuarios
IF OBJECT_ID('Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE Usuarios
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Rol NVARCHAR(20) NOT NULL
    );
END
GO


-- Productos
IF OBJECT_ID('Productos', 'U') IS NULL
BEGIN
    CREATE TABLE Productos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(150) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        Precio DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL,
        TipoProductoId INT NOT NULL,

        CONSTRAINT FK_Productos_TiposProducto
            FOREIGN KEY (TipoProductoId)
            REFERENCES TiposProducto(Id)
    );
END
GO


-- Indice para la relacion con tipo de producto
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Productos_TipoProductoId'
      AND object_id = OBJECT_ID('Productos')
)
BEGIN
    CREATE INDEX IX_Productos_TipoProductoId
        ON Productos(TipoProductoId);
END
GO


-- Tipos de producto iniciales
IF NOT EXISTS (SELECT 1 FROM TiposProducto)
BEGIN
    SET IDENTITY_INSERT TiposProducto ON;

    INSERT INTO TiposProducto (Id, Nombre)
    VALUES
        (1, 'Electrónica'),
        (2, 'Hogar'),
        (3, 'Oficina');

    SET IDENTITY_INSERT TiposProducto OFF;
END
GO


-- Usuario administrador
IF NOT EXISTS (
    SELECT 1
    FROM Usuarios
    WHERE Email = 'admin@serfinsa.com'
)
BEGIN
    SET IDENTITY_INSERT Usuarios ON;

    INSERT INTO Usuarios (Id, Email, PasswordHash, Rol)
    VALUES
    (
        1,
        'admin@serfinsa.com',
        '$2a$11$OmBcs9.cT6hz7PpqAiXpcuFPYQwWHmdOH.nQMrEtZuXvUPJr6xbtO',
        'Admin'
    );

    SET IDENTITY_INSERT Usuarios OFF;
END
GO


-- Usuario de solo lectura
IF NOT EXISTS (
    SELECT 1
    FROM Usuarios
    WHERE Email = 'user@serfinsa.com'
)
BEGIN
    SET IDENTITY_INSERT Usuarios ON;

    INSERT INTO Usuarios (Id, Email, PasswordHash, Rol)
    VALUES
    (
        2,
        'user@serfinsa.com',
        '$2a$11$YyMqjEZzYKEMqf3C6zc.7uxXDs6OEgEZM/eNizxILR2zkWKxcYOZq',
        'User'
    );

    SET IDENTITY_INSERT Usuarios OFF;
END
GO


