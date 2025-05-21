-- Wait for SQL Server to start up
WAITFOR DELAY '00:00:15';

-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'NotesApp')
BEGIN
    CREATE DATABASE NotesApp;
END
GO

USE NotesApp;
GO

-- Create Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
CREATE TABLE Users (
                       Id INT IDENTITY(1,1) PRIMARY KEY,
                       Username NVARCHAR(50) NOT NULL UNIQUE,
                       PasswordHash NVARCHAR(MAX) NOT NULL,
                       CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
END
GO

-- Create Notes table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notes')
BEGIN
CREATE TABLE Notes (
                       Id INT IDENTITY(1,1) PRIMARY KEY,
                       Title NVARCHAR(100) NOT NULL,
                       Content NVARCHAR(MAX) NULL,
                       CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                       UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
                       UserId INT NOT NULL,
                       FOREIGN KEY (UserId) REFERENCES Users(Id)
);
END
GO

-- Add some test data (optional)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'testuser')
BEGIN
    -- Password is 'password123' (you would use proper hashing in real app)
INSERT INTO Users (Username, PasswordHash, CreatedAt)
VALUES ('testuser', 'test-hash-value', GETDATE());

DECLARE @UserId INT = SCOPE_IDENTITY();

INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt, UserId)
VALUES
    ('Welcome Note', 'This is your first note. Start organizing your thoughts!', GETDATE(), GETDATE(), @UserId),
    ('Shopping List', 'Milk\nEggs\nBread\nCoffee', GETDATE(), GETDATE(), @UserId),
    ('Project Ideas', 'Build a personal website\nLearn a new programming language\nCreate a mobile app', GETDATE(), GETDATE(), @UserId);
END
GO
