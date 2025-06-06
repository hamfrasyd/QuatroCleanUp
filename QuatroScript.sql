USE mbuzinous_com_db_quatro_db;
GO

-- ### Step 1: Create Tables without inline NOT NULL or DEFAULT constraints
CREATE TABLE Locations (
    Id INT IDENTITY(1,1),
    Latitude DECIMAL(10,7),
    Longitude DECIMAL(10,7)
);
GO

CREATE TABLE Roles (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(MAX)
);
GO

CREATE TABLE Statuses (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(MAX)
);
GO

CREATE TABLE AvatarPictures (
    Id INT IDENTITY(1,1),
    AvatarPictureData VARBINARY(MAX)
);
GO

CREATE TABLE Users (
    UserId INT IDENTITY(1,1),
    Name NVARCHAR(100),
    Email NVARCHAR(255),
    PasswordHash NVARCHAR(MAX),
    RoleId INT,
    CreatedDate DATETIME2,
	AvatarPictureId INT
);
GO

CREATE TABLE Events (
    EventId INT IDENTITY(1,1),
    Title NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    StartTime DATETIME2,
    EndTime DATETIME2,
    FamilyFriendly BIT,
	Participants INT,
    TrashCollected DECIMAL(18,0),
    StatusId INT,
    LocationId INT
);
GO

CREATE TABLE EventAttendances (
    EventId INT,
    UserId INT,
    CheckIn BIT,
    CreatedDate DATETIME2
);
GO

CREATE TABLE Pictures (
    PictureId INT IDENTITY(1,1),
    EventId INT,
    PictureData VARBINARY(MAX),
    Description NVARCHAR(500)
);
GO

-- ### Step 2: Add NOT NULL Constraints

ALTER TABLE Locations
ALTER COLUMN Id INT NOT NULL;
GO

ALTER TABLE Locations
ALTER COLUMN Latitude DECIMAL(10,7) NOT NULL;
GO

ALTER TABLE Locations
ALTER COLUMN Longitude DECIMAL(10,7) NOT NULL;
GO

ALTER TABLE Roles
ALTER COLUMN Id INT NOT NULL;
GO
ALTER TABLE Roles
ALTER COLUMN Name NVARCHAR(MAX) NOT NULL;
GO

ALTER TABLE Statuses
ALTER COLUMN Id INT NOT NULL;
GO
ALTER TABLE Statuses
ALTER COLUMN Name NVARCHAR(MAX) NOT NULL;
GO

ALTER TABLE AvatarPictures
ALTER COLUMN Id INT NOT NULL;
GO
ALTER TABLE AvatarPictures
ALTER COLUMN AvatarPictureData VARBINARY(MAX) NOT NULL;
GO

ALTER TABLE Users
ALTER COLUMN UserId INT NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN Name NVARCHAR(100) NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN Email NVARCHAR(255) NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN PasswordHash NVARCHAR(MAX) NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN RoleId INT NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN CreatedDate DATETIME2 NOT NULL;
GO
ALTER TABLE Users
ALTER COLUMN AvatarPictureId INT NOT NULL;
GO

ALTER TABLE Events
ALTER COLUMN EventId INT NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN Title NVARCHAR(MAX) NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN Description NVARCHAR(MAX) NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN StartTime DATETIME2 NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN EndTime DATETIME2 NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN FamilyFriendly BIT NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN TrashCollected DECIMAL(18,0) NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN StatusId INT NOT NULL;
GO
ALTER TABLE Events
ALTER COLUMN LocationId INT NOT NULL;
GO

ALTER TABLE EventAttendances
ALTER COLUMN EventId INT NOT NULL;
GO
ALTER TABLE EventAttendances
ALTER COLUMN UserId INT NOT NULL;
GO
ALTER TABLE EventAttendances
ALTER COLUMN CheckIn BIT NOT NULL;
GO
ALTER TABLE EventAttendances
ALTER COLUMN CreatedDate DATETIME2 NOT NULL;
GO

ALTER TABLE Pictures
ALTER COLUMN PictureId INT NOT NULL;
GO
ALTER TABLE Pictures
ALTER COLUMN EventId INT NOT NULL;
GO
ALTER TABLE Pictures
ALTER COLUMN PictureData VARBINARY(MAX) NOT NULL;
GO

-- ### Step 3: Add DEFAULT Constraints
ALTER TABLE Users
ADD CONSTRAINT DF_Users_CreatedDate DEFAULT GETUTCDATE() FOR CreatedDate;
GO

ALTER TABLE Users
ADD CONSTRAINT DF_Users_AvatarPictureId DEFAULT 1 FOR AvatarPictureId;
GO


ALTER TABLE EventAttendances
ADD CONSTRAINT DF_EventAttendances_CreatedDate DEFAULT GETUTCDATE() FOR CreatedDate;
GO

-- ### Step 4: Add Other Constraints
-- Primary Keys
ALTER TABLE Locations ADD CONSTRAINT PK_Locations PRIMARY KEY (Id);
GO
ALTER TABLE Roles ADD CONSTRAINT PK_Roles PRIMARY KEY (Id);
GO
ALTER TABLE Statuses ADD CONSTRAINT PK_Statuses PRIMARY KEY (Id);
GO
ALTER TABLE AvatarPictures ADD CONSTRAINT PK_AvatarPictures PRIMARY KEY (Id);
GO
ALTER TABLE Users ADD CONSTRAINT PK_Users PRIMARY KEY (UserId);
GO
ALTER TABLE Events ADD CONSTRAINT PK_Events PRIMARY KEY (EventId);
GO
ALTER TABLE Pictures ADD CONSTRAINT PK_Pictures PRIMARY KEY (PictureId);
GO
ALTER TABLE EventAttendances ADD CONSTRAINT PK_EventAttendances PRIMARY KEY (EventId, UserId);
GO


-- Foreign Keys
ALTER TABLE Users ADD CONSTRAINT FK_Users_Roles_RoleId  FOREIGN KEY (RoleId) REFERENCES Roles(Id);
GO
ALTER TABLE Users ADD CONSTRAINT FK_Users_AvatarPictures_AvatarPictureId  FOREIGN KEY (AvatarPictureId) REFERENCES AvatarPictures(Id);
GO
ALTER TABLE Events ADD CONSTRAINT FK_Events_Statuses_StatusId  FOREIGN KEY (StatusId) REFERENCES Statuses(Id);
GO
ALTER TABLE Events ADD CONSTRAINT FK_Events_Locations_LocationId  FOREIGN KEY (LocationId) REFERENCES Locations(Id);
GO
ALTER TABLE EventAttendances ADD CONSTRAINT FK_EventAttendances_Events_EventId FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE;
GO
ALTER TABLE EventAttendances ADD CONSTRAINT FK_EventAttendances_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE;
GO
ALTER TABLE Pictures ADD CONSTRAINT FK_Pictures_Events_EventId FOREIGN KEY (EventId) REFERENCES Events(EventId);
GO


-- Unique Constraints
ALTER TABLE Users ADD CONSTRAINT UQ_Email UNIQUE (Email);
GO
ALTER TABLE EventAttendances ADD CONSTRAINT UQ_EventId_UserId UNIQUE (EventId, UserId);
GO


-- Check Constraints
ALTER TABLE Events ADD CONSTRAINT CHK_EndTimeAfterStartTime CHECK (EndTime > StartTime);
GO

-- ### Step 5: Create Triggers and Procedures
CREATE TRIGGER TR_Events_UpdateStatus
ON Events
AFTER INSERT, UPDATE
AS
BEGIN
    -- Prevent extra resultsets from interfering with client apps
    SET NOCOUNT ON;

    -- Recompute status for rows that were just inserted or updated,
    -- but only if their previous status was not “Canceled”.
    UPDATE eventTable
    SET eventTable.StatusId = CASE
        -- 1) If it’s already Canceled (4), leave it as Canceled
        WHEN eventTable.StatusId = 4 THEN 4

        -- 2) If current UTC time is before the event’s start, mark Upcoming (1)
        WHEN GETUTCDATE() < eventTable.StartTime THEN 1

        -- 3) If current UTC time is between start and end, mark Ongoing (2)
        WHEN GETUTCDATE() BETWEEN eventTable.StartTime AND eventTable.EndTime THEN 2

        -- 4) Otherwise (past end), mark Completed (3)
        ELSE 3
    END
    FROM Events AS eventTable

    -- only touch rows that were just inserted or updated
    INNER JOIN inserted AS newRows
        ON eventTable.EventId = newRows.EventId

    -- keep track of what the status was before the change
    INNER JOIN deleted AS oldRows
        ON eventTable.EventId = oldRows.EventId

    -- only recalculate if the *old* status was non‑canceled (< 4)
    WHERE oldRows.StatusId < 4;
END;
GO


CREATE PROCEDURE DeleteOldEventAttendances AS
BEGIN
    -- Permanently delete event attendances that have been previously marked as soft-deleted.
    -- The TR_EventAttendances_InsteadOfDelete trigger handles setting isDeleted = 1.
    -- This procedure cleans up those marked records.
    DELETE FROM EventAttendances
    WHERE CreatedDate < DATEADD(YEAR, -2, GETUTCDATE());
END;
GO

CREATE PROCEDURE UpdateEventStatuses
AS
BEGIN
    -- 1) Prevent extra resultsets from interfering:
    SET NOCOUNT ON;

    -- 2) Update only non‑canceled events:
    UPDATE eventsTable
    SET eventsTable.StatusId = CASE
        -- a) If already “Canceled” (4), leave it at 4
        WHEN eventsTable.StatusId = 4 THEN 4 

        -- b) If current UTC time is before StartTime, set “Upcoming” (1)
        WHEN GETUTCDATE() < eventsTable.StartTime THEN 1

        -- c) If current UTC time is between StartTime and EndTime, set “Ongoing” (2)
        WHEN GETUTCDATE() BETWEEN eventsTable.StartTime AND eventsTable.EndTime THEN 2

        -- d) Otherwise (if current UTC time is after EndTime), set “Completed” (3)
        ELSE 3
    END
    FROM Events AS eventsTable
    WHERE eventsTable.StatusId < 4; -- Only recalculate statuses that aren’t already Canceled
END;
GO

-- ### Step 6: Insert Initial Data
INSERT INTO Roles (Name) VALUES ('Admin'), ('User');
GO
INSERT INTO Statuses (Name) VALUES ('Upcoming'), ('Ongoing'), ('Completed'), ('Canceled');
GO

-- Dummy data for AvatarPictures
INSERT INTO AvatarPictures (AvatarPictureData)
VALUES
(0xDEADBEEF),  -- Id = 1
(0xAABBCCDD),  -- Id = 2
(0x01020304),  -- Id = 3
(0x11111111),  -- Id = 4
(0x22222222),  -- Id = 5
(0x33333333),  -- Id = 6
(0x44444444),  -- Id = 7
(0x55555555),  -- Id = 8
(0x66666666),  -- Id = 9
(0x77777777);  -- Id = 10
GO
-- ### Step 8: Insert Dummy Data
-- Insert Locations
INSERT INTO Locations (Latitude, Longitude)
VALUES 
(42.2890000, 18.8330000),  -- 1. Budva Beach, Montenegro
(42.4410000, 19.2590000),  -- 2. Podgorica Community Center
(42.4240000, 18.7240000),  -- 3. Tivat City Square
(42.3730000, 18.9020000),  -- 4. Njegošev Park, Cetinje
(42.0960000, 19.1020000),  -- 5. Bar Eco Hub
(42.4410000, 19.2590000),  -- 6. Morača Riverbank, Podgorica
(42.4240000, 18.7710000),  -- 7. Kotor Culinary Studio
(42.4710000, 18.5310000),  -- 8. Herceg Novi Culture Hall
(42.7840000, 18.9570000),  -- 9. Nikšić Public Library Garden
(42.4410000, 19.2590000);  -- 10. Cineplex Delta City
GO

-- Insert Events (Note: Removed NumberOfAttendees as it’s not in the table definition)
INSERT INTO Events (Title, Description, StartTime, EndTime, FamilyFriendly, TrashCollected, StatusId, LocationId)
VALUES 
('Beach Clean-Up at Budva', 
 'Join us for a community beach clean-up at Budva to reduce plastic pollution and raise awareness about ocean waste. Gloves, bags, and refreshments will be provided.', 
 '2025-02-02 12:00:00', '2025-02-02 14:00:00', 1, 0, 1, 1),
('Zero Waste Workshop for Beginners', 
 'Learn the basics of zero waste living, including DIY cleaning products, composting, and sustainable shopping tips.', 
 '2025-04-13 12:00:00', '2025-04-13 14:00:00', 1, 0, 1, 2),
('Green Market Swap Day', 
 'A fun swap meet where participants bring gently used items to trade instead of throwing them away. Includes upcycling demos.', 
 '2025-04-14 12:00:00', '2025-04-14 14:00:00', 1, 0, 1, 3),
('Zero Waste Picnic in the Park', 
 'Bring your own reusable containers and enjoy a waste-free picnic with fellow environmental enthusiasts. Games and activities for kids included.', 
 '2025-04-15 12:00:00', '2025-04-15 14:00:00', 1, 0, 1, 4),
('Waste Audit Challenge', 
 'A hands-on event where participants learn how to audit their weekly trash and identify ways to reduce waste at home.', 
 '2025-05-10 18:00:00', '2025-05-10 20:00:00', 0, 0, 1, 5),
('Riverbank Restoration & Clean-Up', 
 'Help clean up and restore the banks of the Morača River. Volunteers will also plant native trees and learn about erosion control.', 
 '2025-05-30 09:00:00', '2025-05-30 13:00:00', 1, 0, 1, 6),
('Zero Waste Cooking Class', 
 'Discover how to cook delicious meals with minimal waste using local, seasonal ingredients. Includes a tasting session.', 
 '2025-06-20 16:30:00', '2025-06-20 19:00:00', 0, 0, 1, 7),
('Sustainable Fashion Talk & Swap', 
 'Learn from eco-fashion experts about reducing textile waste, then participate in a clothing swap to refresh your wardrobe sustainably.', 
 '2025-07-05 18:00:00', '2025-07-05 20:30:00', 1, 0, 1, 8),
('Kids’ Eco-Art Day', 
 'Children create art from recycled materials while learning about waste reduction in a playful environment.', 
 '2025-08-28 10:00:00', '2025-08-28 13:00:00', 1, 0, 1, 9),
('Zero Waste Documentary Screening & Discussion', 
 'Watch a powerful documentary on the global waste crisis followed by a moderated discussion with local sustainability experts.', 
 '2025-09-12 19:00:00', '2025-09-12 21:30:00', 0, 0, 1, 10);
GO

INSERT INTO Pictures (EventId, PictureData, Description)
VALUES
(1, 0x00000001, N'Budva – strand før oprydning'),
(1, 0x00000002, N'Workshop – flyer'),
(1, 0x00000003, N'Swap Day – banner'),
(1, 0x00000004, N'Picnic – parkstemning'),
(5, 0x00000005, N'Waste Audit – affaldseksempel'),
(6, 0x00000006, N'Riverbank – frivillige i aktion'),
(7, 0x00000007, N'Cooking Class – ingredienser'),
(8, 0x00000008, N'Fashion Talk – før/efter outfit'),
(9, 0x00000009, N'Eco-Art Day – børnekrea'),
(10,0x0000000A, N'Dokumentar – plakat');
GO
