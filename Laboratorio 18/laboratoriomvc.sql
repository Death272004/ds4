Use Master 
Go 

IF DB_ID('laboratoriomvc') IS NOT NULL
BEGIN
    ALTER DATABASE laboratoriomvc SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE laboratoriomvc;
END
GO

CREATE DATABASE laboratoriomvc;
GO

USE laboratoriomvc;
GO

CREATE TABLE [User] (
    id INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(50) NOT NULL
);
GO

INSERT INTO [User] (email, password) VALUES
('usuario1@test.com', '123456'),
('admin@test.com', 'admin123');
GO

SELECT * FROM [User];
