USE master
GO 
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'MonitorPuertos')
BEGIN
    ALTER DATABASE MonitorPuertos SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MonitorPuertos;
END
GO

CREATE DATABASE MonitorPuertos;
GO

USE MonitorPuertos;
GO

-- Tabla de tipos de periféricos
CREATE TABLE TiposPerifericos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Icono NVARCHAR(100),
    Descripcion NVARCHAR(255)
);

-- Tabla de periféricos detectados
CREATE TABLE Perifericos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TipoID INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    DeviceID NVARCHAR(255) NOT NULL UNIQUE,
    Fabricante NVARCHAR(100),
    Puerto NVARCHAR(50),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    UltimaActividad DATETIME,
    Activo BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_Perifericos_Tipos
        FOREIGN KEY (TipoID) REFERENCES TiposPerifericos(ID)
);

-- Tabla de actividad de periféricos
CREATE TABLE ActividadPerifericos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PerifericoID INT NOT NULL,
    TipoEvento NVARCHAR(50) NOT NULL, -- Conectado, Desconectado, EnUso, Error
    Estado NVARCHAR(20) NOT NULL, -- Online, Offline, Ocupado, Error
    FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    Detalles NVARCHAR(500),
    DatosRaw NVARCHAR(MAX),
    Validado BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_ActividadPerifericos_Perifericos
        FOREIGN KEY (PerifericoID) REFERENCES Perifericos(ID)
);

-- Tabla de puertos físicos de la computadora
CREATE TABLE Puertos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    TipoPuerto NVARCHAR(50) NOT NULL, -- USB-A, USB-C, HDMI, Audio-Jack, SD-Card, Power
    Ubicacion NVARCHAR(100), -- Izquierda, Derecha, Trasero, Frontal
    Descripcion NVARCHAR(255),
    EstaConectado BIT NOT NULL DEFAULT 0, -- Si actualmente tiene algo conectado
    DispositivoConectado NVARCHAR(255), -- Nombre del dispositivo conectado
    UltimaDeteccion DATETIME
);

-- Tabla de uso de puertos
CREATE TABLE UsoPuertos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PuertoID INT NOT NULL,
    PerifericoID INT,
    Estado NVARCHAR(20) NOT NULL, -- Ocupado, Libre, Error
    FechaInicio DATETIME NOT NULL DEFAULT GETDATE(),
    FechaFin DATETIME,
    Duracion INT, -- en segundos
    Validado BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_UsoPuertos_Puertos
        FOREIGN KEY (PuertoID) REFERENCES Puertos(ID),
    CONSTRAINT FK_UsoPuertos_Perifericos
        FOREIGN KEY (PerifericoID) REFERENCES Perifericos(ID)
);

-- Índices para optimización
CREATE INDEX IDX_ActividadPerifericos_FechaHora ON ActividadPerifericos(FechaHora DESC);
CREATE INDEX IDX_ActividadPerifericos_Periferico ON ActividadPerifericos(PerifericoID);
CREATE INDEX IDX_Perifericos_Activo ON Perifericos(Activo);
CREATE INDEX IDX_UsoPuertos_FechaInicio ON UsoPuertos(FechaInicio DESC);
CREATE INDEX IDX_UsoPuertos_Estado ON UsoPuertos(Estado);

-- Insertar tipos de periféricos comunes
INSERT INTO TiposPerifericos (Nombre, Icono, Descripcion) VALUES
('Mouse', 'mouse.png', 'Ratón o Mouse'),
('Teclado', 'keyboard.png', 'Teclado'),
('Monitor', 'monitor.png', 'Monitor o Pantalla'),
('USB', 'usb.png', 'Dispositivo USB Genérico'),
('Impresora', 'printer.png', 'Impresora'),
('Auriculares', 'headphones.png', 'Auriculares o Audífonos'),
('Camara', 'webcam.png', 'Cámara Web'),
('Microfono', 'microphone.png', 'Micrófono'),
('Disco', 'harddrive.png', 'Disco Externo'),
('Red', 'network.png', 'Adaptador de Red');

GO

-- Insertar puertos físicos típicos de una laptop
INSERT INTO Puertos (Nombre, TipoPuerto, Ubicacion, Descripcion) VALUES
('USB-A Puerto 1', 'USB-A', 'Lateral Izquierdo', 'Puerto USB tipo A estándar'),
('USB-A Puerto 2', 'USB-A', 'Lateral Derecho', 'Puerto USB tipo A estándar'),
('USB-C Puerto 1', 'USB-C', 'Lateral Derecho', 'Puerto USB tipo C multifunción'),
('Puerto HDMI', 'HDMI', 'Lateral Derecho', 'Puerto HDMI para video'),
('Puerto Carga', 'Power', 'Lateral Izquierdo', 'Puerto de carga de energía'),
('Jack Audio', 'Audio-Jack', 'Lateral Derecho', 'Puerto jack 3.5mm para audífonos'),
('Lector SD', 'SD-Card', 'Lateral Derecho', 'Lector de tarjetas SD');

GO

-- Vista para obtener el estado actual de periféricos
CREATE VIEW vw_EstadoActualPerifericos AS
SELECT 
    p.ID,
    p.Nombre,
    p.DeviceID,
    p.Puerto,
    tp.Nombre AS TipoPerifico,
    tp.Icono,
    p.Activo,
    p.UltimaActividad,
    ISNULL(
        (SELECT TOP 1 Estado 
         FROM ActividadPerifericos 
         WHERE PerifericoID = p.ID AND Validado = 1
         ORDER BY FechaHora DESC), 
        'Desconocido'
    ) AS EstadoActual,
    ISNULL(
        (SELECT TOP 1 TipoEvento 
         FROM ActividadPerifericos 
         WHERE PerifericoID = p.ID AND Validado = 1
         ORDER BY FechaHora DESC), 
        'Sin Eventos'
    ) AS UltimoEvento
FROM Perifericos p
INNER JOIN TiposPerifericos tp ON p.TipoID = tp.ID;

GO

-- Vista para puertos usados recientemente
CREATE VIEW vw_PuertosRecientes AS
SELECT TOP 50
    up.ID,
    pt.Nombre AS NombrePuerto,
    pt.TipoPuerto,
    pt.Ubicacion,
    p.Nombre AS Periferico,
    tp.Nombre AS TipoPeriferico,
    up.Estado,
    up.FechaInicio,
    up.FechaFin,
    up.Duracion,
    CASE 
        WHEN up.FechaFin IS NULL THEN 1 
        ELSE 0 
    END AS EnUso
FROM UsoPuertos up
INNER JOIN Puertos pt ON up.PuertoID = pt.ID
LEFT JOIN Perifericos p ON up.PerifericoID = p.ID
LEFT JOIN TiposPerifericos tp ON p.TipoID = tp.ID
WHERE up.Validado = 1
ORDER BY up.FechaInicio DESC;

GO

-- Procedimiento almacenado para registrar actividad validada
CREATE PROCEDURE sp_RegistrarActividadValidada
    @PerifericoID INT,
    @TipoEvento NVARCHAR(50),
    @Estado NVARCHAR(20),
    @Detalles NVARCHAR(500) = NULL,
    @DatosRaw NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Insertar actividad validada
    INSERT INTO ActividadPerifericos (PerifericoID, TipoEvento, Estado, Detalles, DatosRaw, Validado)
    VALUES (@PerifericoID, @TipoEvento, @Estado, @Detalles, @DatosRaw, 1);
    
    -- Actualizar última actividad del periférico
    UPDATE Perifericos 
    SET UltimaActividad = GETDATE(),
        Activo = CASE WHEN @Estado IN ('Online', 'Ocupado', 'EnUso') THEN 1 ELSE 0 END
    WHERE ID = @PerifericoID;
END;

GO

-- Procedimiento para registrar uso de puerto
CREATE PROCEDURE sp_RegistrarUsoPuerto
    @PuertoID INT,
    @PerifericoID INT = NULL,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cerrar uso anterior si existe uno abierto
    UPDATE UsoPuertos
    SET FechaFin = GETDATE(),
        Duracion = DATEDIFF(SECOND, FechaInicio, GETDATE())
    WHERE PuertoID = @PuertoID 
        AND FechaFin IS NULL;
    
    -- Registrar nuevo uso si el puerto está ocupado
    IF @Estado = 'Ocupado'
    BEGIN
        INSERT INTO UsoPuertos (PuertoID, PerifericoID, Estado, Validado)
        VALUES (@PuertoID, @PerifericoID, @Estado, 1);
    END
END;

GO
