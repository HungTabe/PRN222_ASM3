-- Seed data for EventManagementDb
-- Run this script in SQL Server Management Studio or a similar tool
-- Ensure the database and tables are created via EF Core migrations before running

-- Disable foreign key constraints to allow data insertion in any order
ALTER TABLE Attendees NOCHECK CONSTRAINT ALL;
ALTER TABLE Events NOCHECK CONSTRAINT ALL;

-- Clear existing data (optional, comment out if you want to append data)
DELETE FROM Attendees;
DELETE FROM Events;
DELETE FROM EventCategories;
DELETE FROM Users;

-- Reset identity columns (optional, ensures IDs start from 1)
DBCC CHECKIDENT ('Attendees', RESEED, 0);
DBCC CHECKIDENT ('Events', RESEED, 0);
DBCC CHECKIDENT ('EventCategories', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);

-- Insert Users
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES 
    ('admin1', 'Admin123', 'John Admin', 'admin1@example.com', 'Admin'),
    ('user1', 'User123', 'Alice Smith', 'alice@example.com', 'User'),
    ('user2', 'User123', 'Bob Johnson', 'bob@example.com', 'User'),
    ('user3', 'User123', 'Carol White', 'carol@example.com', 'User'),
    ('manager1', 'Manager123', 'David Brown', 'david@example.com', 'Admin');

-- Insert EventCategories
INSERT INTO EventCategories (CategoryName)
VALUES 
    ('Conference'),
    ('Workshop'),
    ('Concert'),
    ('Seminar'),
    ('Networking');

-- Insert Events
INSERT INTO Events (Title, Description, Location, StartTime, EndTime, CategoryID)
VALUES 
    ('AI Conference 2025', 'Explore the future of artificial intelligence.', 'New York Convention Center', '2025-07-10 09:00:00', '2025-07-10 17:00:00', 1),
    ('Web Development Workshop', 'Hands-on coding with JavaScript and ASP.NET.', 'Online', '2025-06-20 10:00:00', '2025-06-20 14:00:00', 2),
    ('Rock Concert', 'Live performance by top bands.', 'Madison Square Garden', '2025-08-15 19:00:00', '2025-08-15 23:00:00', 3),
    ('Data Science Seminar', 'Learn about machine learning techniques.', 'Boston University', '2025-06-25 13:00:00', '2025-06-25 16:00:00', 4),
    ('Tech Networking Event', 'Connect with industry professionals.', 'San Francisco Tech Hub', '2025-07-05 18:00:00', '2025-07-05 21:00:00', 5),
    ('No Category Event', 'An event without a category.', 'Local Community Center', '2025-06-30 15:00:00', '2025-06-30 17:00:00', NULL);

-- Insert Attendees
INSERT INTO Attendees (UserID, EventID, Name, Email, RegistrationTime)
VALUES 
    (2, 1, 'Alice Smith', 'alice@example.com', '2025-06-01 10:00:00'),
    (3, 1, 'Bob Johnson', 'bob@example.com', '2025-06-02 12:00:00'),
    (4, 1, 'Carol White', 'carol@example.com', '2025-06-03 15:00:00'),
    (2, 2, 'Alice Smith', 'alice@example.com', '2025-06-05 09:00:00'),
    (3, 3, 'Bob Johnson', 'bob@example.com', '2025-06-10 14:00:00'),
    (4, 4, 'Carol White', 'carol@example.com', '2025-06-12 11:00:00'),
    (2, 5, 'Alice Smith', 'alice@example.com', '2025-06-15 16:00:00');

-- Re-enable foreign key constraints
ALTER TABLE Attendees CHECK CONSTRAINT ALL;
ALTER TABLE Events CHECK CONSTRAINT ALL;

-- Verify the inserted data
SELECT * FROM Users;
SELECT * FROM EventCategories;
SELECT * FROM Events;
SELECT * FROM Attendees;