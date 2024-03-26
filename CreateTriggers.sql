-- First trigger: Event_Delete
CREATE TRIGGER [Event_Delete]
    ON [dbo].[Events]
    AFTER DELETE
AS
BEGIN
    DELETE FROM Comments WHERE EventId IN (SELECT deleted.Id FROM deleted);
END;
GO

-- Second trigger: User_Delete
CREATE TRIGGER [User_Delete]
    ON [dbo].[AspNetUsers]
    AFTER DELETE
AS
BEGIN
    DELETE FROM Categories WHERE OrganizerId IN (SELECT deleted.Id FROM deleted);
    DELETE FROM Events WHERE UserId IN (SELECT deleted.Id FROM deleted);
    DELETE FROM Comments WHERE UserId IN (SELECT deleted.Id FROM deleted);
END;
GO

-- Third trigger: Category_Delete
CREATE TRIGGER [Category_Delete]
    ON [dbo].[Categories]
    AFTER DELETE
AS
BEGIN
    DELETE FROM Events WHERE CategoryId IN (SELECT deleted.Id FROM deleted);
END;
GO
