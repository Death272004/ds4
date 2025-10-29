USE master;  -- Cambiar a master primero
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'Proyecto')
BEGIN
    ALTER DATABASE Proyecto SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Proyecto;
END
GO

CREATE DATABASE Proyecto;
GO

USE Proyecto;
GO

CREATE TABLE HISTORIAL (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    OPERACION NVARCHAR(100) NOT NULL,
    RESULTADO NVARCHAR(50) NOT NULL,
    TIPOS NVARCHAR(20) NULL,
    FECHA DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Datos de prueba
INSERT INTO HISTORIAL (OPERACION, RESULTADO, TIPOS)
VALUES
    ('5 + 3', '8', 'Suma'),
    ('10 - 2', '8', 'Resta'),
    ('4 × 2', '8', 'Multiplicacion'),
    ('16 ÷ 2', '8', 'Division'),
    ('9²', '81', 'Cuadrado'),
    ('√16', '4', 'Raiz'),
    ('5 + 3 × 2', '11', 'Mixta');
GO

SELECT * FROM HISTORIAL;