USE [CommentApplication]

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](500) NOT NULL,
	[CreatedTime] DateTimeOffset,
	[UpdatedTime] DateTimeOffset,
	[SecretCode] [varchar](50),
	[IsLoggedIn] [BIT]
)
GO


--INSERT 
--	[dbo].[Users] ([Email], [Password],[CreatedTime], [SecretCode]) 
--VALUES 
--	(N'testuser01@commentapp.com', N'testuser')
--GO

CREATE TABLE [dbo].[UserComments](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Comment] [varchar](MAX) NOT NULL,
	[CreatedTime] DateTimeOffset,
	[UpdatedTime] DateTimeOffset,
	[UserId] int NOT NULL FOREIGN KEY REFERENCES Users(ID)
)
GO


USE [MASTER]
GO
ALTER DATABASE [CommentApplication] SET  READ_WRITE 
GO



Select * from Users

Insert into UserComments (Comment,CreatedTime,UserId)
values ('Test',SYSDATETIMEOFFSET(),1)