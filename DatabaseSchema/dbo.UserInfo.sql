CREATE TABLE [dbo].[UserInfo]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY , 
	[Password] NCHAR(32) NOT NULL,
    [Name] NVARCHAR(128) NOT NULL, 
    [Gender] NCHAR(10) NOT NULL, 
    [AboutMe] NTEXT NULL
)
