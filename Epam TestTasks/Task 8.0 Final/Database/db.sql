USE [master]
GO
/****** Object:  Database [SocialNetwork]    Script Date: 07.10.2020 22:20:57 ******/
CREATE DATABASE [SocialNetwork]
 CONTAINMENT = NONE
GO
ALTER DATABASE [SocialNetwork] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SocialNetwork].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SocialNetwork] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SocialNetwork] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SocialNetwork] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SocialNetwork] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SocialNetwork] SET ARITHABORT OFF 
GO
ALTER DATABASE [SocialNetwork] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SocialNetwork] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SocialNetwork] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SocialNetwork] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SocialNetwork] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SocialNetwork] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SocialNetwork] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SocialNetwork] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SocialNetwork] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SocialNetwork] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SocialNetwork] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SocialNetwork] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SocialNetwork] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SocialNetwork] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SocialNetwork] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SocialNetwork] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SocialNetwork] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SocialNetwork] SET RECOVERY FULL 
GO
ALTER DATABASE [SocialNetwork] SET  MULTI_USER 
GO
ALTER DATABASE [SocialNetwork] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SocialNetwork] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SocialNetwork] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SocialNetwork] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SocialNetwork] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'SocialNetwork', N'ON'
GO
ALTER DATABASE [SocialNetwork] SET QUERY_STORE = OFF
GO
USE [SocialNetwork]
GO
/****** Object:  Table [dbo].[chats]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chats](
	[idChat] [int] IDENTITY(1,1) NOT NULL,
	[idPerson1] [int] NOT NULL,
	[idPerson2] [int] NOT NULL,
	[unreaded] [int] NULL,
 CONSTRAINT [PK_chats] PRIMARY KEY CLUSTERED 
(
	[idChat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[messages]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[messages](
	[idMessage] [int] IDENTITY(1,1) NOT NULL,
	[idChat] [int] NOT NULL,
	[IdSender] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[text] [nvarchar](1500) NOT NULL,
 CONSTRAINT [PK_messages_1] PRIMARY KEY CLUSTERED 
(
	[idMessage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[relationships]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[relationships](
	[person1] [int] NOT NULL,
	[person2] [int] NOT NULL,
	[inventation] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rolenames]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rolenames](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_rolenames] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[idUser] [int] NOT NULL,
	[role] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](320) NOT NULL,
	[password] [nvarchar](64) NULL,
	[name] [nvarchar](250) NOT NULL,
	[birth] [date] NOT NULL,
	[statement] [nvarchar](50) NULL,
	[emblem] [nvarchar](250) NULL,
	[blockedBy] [int] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[messages]  WITH CHECK ADD  CONSTRAINT [FK_messages_chats] FOREIGN KEY([idChat])
REFERENCES [dbo].[chats] ([idChat])
GO
ALTER TABLE [dbo].[messages] CHECK CONSTRAINT [FK_messages_chats]
GO
ALTER TABLE [dbo].[roles]  WITH CHECK ADD  CONSTRAINT [FK_roles_users] FOREIGN KEY([idUser])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[roles] CHECK CONSTRAINT [FK_roles_users]
GO
/****** Object:  StoredProcedure [dbo].[AcceptFriendRequest]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AcceptFriendRequest] 
	
	@idUser int,
	@idOpponent int

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE relationships
	SET inventation = NULL
	WHERE (person1 = @idUser and person2 = @idOpponent)
	OR (person2 = @idUser and person1 = @idOpponent);
END
GO
/****** Object:  StoredProcedure [dbo].[AddRoleToUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRoleToUser]

	@email nvarchar(320),
	@role nvarchar(50)

AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @id int;
	DECLARE @idRole int;

	SET @id = (
		SELECT TOP(1) id 
		FROM users 
		WHERE email = @email);

	SET @idRole = (
		SELECT TOP(1) id
		FROM rolenames
		WHERE name = @role);

	INSERT INTO roles(idUser, role)
	VALUES (@id, @idRole);
END
GO
/****** Object:  StoredProcedure [dbo].[ChangePasswordOfUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangePasswordOfUser]

	@id int,
	@password nvarchar(64),
	@oldPassword nvarchar(64)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE users 
	SET password = @password 
	WHERE id = @id
	AND password = @oldPassword;
	SELECT COUNT(id) 
	FROM users 
	WHERE id = @id
	AND password = @password;
END
GO
/****** Object:  StoredProcedure [dbo].[CreateChat]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateChat]

	@id int OUTPUT,
	@idPerson1 int,
	@idPerson2 int

AS
BEGIN
	SET NOCOUNT OFF;

    INSERT INTO chats (idPerson1, idPerson2)
	VALUES (@idPerson1, @idPerson2);
	SELECT @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUser]

	@email nvarchar(320),
	@name nvarchar(250),
	@birth date,
	@id int OUTPUT

AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO users (email, name, birth)
	VALUES (@email, @name, @birth);
	SELECT @id = SCOPE_IDENTITY();
	INSERT INTO roles (idUser, role)
	VALUES (@id, 'user');
END
GO
/****** Object:  StoredProcedure [dbo].[FindUsersByName]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindUsersByName]
	
	@text nvarchar(250),
	@curUserId int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT id, name, birth, statement, emblem, blockedBy
	FROM users
	WHERE name LIKE @text AND id != @curUserId
	AND blockedBy is NULL
	ORDER BY name;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllChatsOfUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllChatsOfUser]

	@id int

AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM chats
	WHERE (
		SELECT COUNT(m.idMessage) 
		FROM messages as m 
		WHERE m.idChat = chats.idChat) = 0 
		AND (idPerson1 = @id OR idPerson2 = @id);

	SELECT c.idChat, u.name, u.emblem, x.name AS sname, m.text, m.date, c.unreaded
	FROM chats as c 
	INNER JOIN messages AS m 
	ON  m.idChat = c.idChat
	INNER JOIN users AS u 
	ON u.id = CASE
		WHEN c.idPerson1 = @id
		THEN c.idPerson2
		WHEN c.idPerson2 = @id
		THEN c.idPerson1
		END
	INNER JOIN users AS x 
	ON m.IdSender = x.id
	WHERE (c.idPerson1 = @id or c.idPerson2 = @id)
	AND m.date = (
		SELECT MAX(date)
		FROM messages 
		WHERE idChat = c.idChat)
	ORDER BY m.date DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllMessagesFromChat]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllMessagesFromChat]
	
	@id int,
	@idReader int

AS
BEGIN
	SET NOCOUNT ON;

	IF 
		(SELECT unreaded 
		FROM chats 
		WHERE idChat = @id) = @idReader
	UPDATE chats 
	SET unreaded = NULL
	WHERE idChat = @id


	SELECT * 
	FROM messages 
	WHERE idChat = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllUsersBesides]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUsersBesides]
	
	@curUserId int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM users 
	WHERE id != @curUserId
	AND blockedBy is NULL
	ORDER BY name;
END
GO
/****** Object:  StoredProcedure [dbo].[GetChatId]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetChatId]
	
	@idUser int,
	@idOpponent int

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT idChat 
	FROM chats 
	WHERE (idPerson1 = @idUser 
		AND idPerson2 = @idOpponent) 
	OR (idPerson2 = @idUser 
		AND idPerson1 = @idOpponent);
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmailCount]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEmailCount]
	
	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(id)
	FROM users 
	WHERE email = @email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetFriendRequests]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFriendRequests]

	@idUser int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.id, u.name, u.birth, u.statement, u.emblem, u.blockedBy
	FROM relationships as r 
	INNER JOIN users AS u 
	ON u.id = r.inventation 
	WHERE (r.person1 = @idUser OR r.person2 = @idUser)
	AND (r.inventation IS NOT NULL AND r.inventation != @idUser);
END
GO
/****** Object:  StoredProcedure [dbo].[GetFriends]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFriends]

@idUser int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.id, u.name, u.birth, u.statement, u.emblem, u.blockedBy
	FROM relationships as r 
	INNER JOIN users AS u 
	ON u.id = 
		CASE 
		WHEN r.person1 = @idUser 
		THEN r.person2 
		WHEN  r.person2 = @idUser 
		THEN r.person1 
		END 
	WHERE (r.person1 = @idUser OR r.person2 = @idUser) 
	AND r.inventation is NULL;
END
GO
/****** Object:  StoredProcedure [dbo].[GetNumOfUsersRoles]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNumOfUsersRoles]

	@email nvarchar(320),
	@role nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(u.email)
	FROM roles as r
	INNER JOIN users as u
	ON r.idUser = u.id
	WHERE u.email = @email
	AND r.role = @role;
END
GO
/****** Object:  StoredProcedure [dbo].[GetPassword]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[GetPassword]
	
	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT password 
	FROM users 
	WHERE email = @email;
END

GO
/****** Object:  StoredProcedure [dbo].[GetRoleOfUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[GetRoleOfUser]

	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT rn.name
	FROM roles AS r
	INNER JOIN users AS u 
	ON r.idUser = u.id 
	INNER JOIN rolenames AS rn
	ON r.role = rn.id
	WHERE u.email = @email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUnreadedChatsCount]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUnreadedChatsCount]

	@id int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(unreaded)
	FROM chats
	WHERE unreaded = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserByEmail]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserByEmail]
	
	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT id, name, birth, statement, emblem, blockedBy
	FROM users 
	WHERE email = @email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserById]
	
	@id int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT id, name, birth, statement, emblem, blockedBy
	FROM users 
	WHERE id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserId]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserId]
	
	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT id 
	FROM users 
	WHERE email = @email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserRegistrationCount]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserRegistrationCount]
	
	@email nvarchar(320)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(email) 
	FROM users 
	WHERE email = @email;
END

GO
/****** Object:  StoredProcedure [dbo].[GetUsersInventations]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsersInventations]

	@idUser int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.id, u.name, u.birth, u.statement, u.emblem, u.blockedBy
	FROM relationships as r
	INNER JOIN users AS u
	ON u.id = CASE
		 WHEN r.person1 = @idUser 
		 THEN r.person2 
		 WHEN  r.person2 = @idUser 
		 THEN r.person1
		 END
	WHERE (r.person1 = @idUser OR r.person2 = @idUser)
	AND (r.inventation IS NOT NULL AND r.inventation = @idUser);
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveChat]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveChat]

	@id int

AS
BEGIN
	SET NOCOUNT OFF;

    DELETE FROM messages 
	WHERE idChat = @id;
	DELETE FROM chats 
	WHERE idChat = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveFriend]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveFriend]
	
	@idUser int,
	@idOpponent int

AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM relationships 
	WHERE (person1 = @idUser AND person2 = @idOpponent)
	OR (person2 = @idUser AND person1 = @idOpponent);
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveMessage]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveMessage]
	
	@idMessage int

AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @idChat int;

	SET @idChat = (
		SELECT idChat 
		FROM messages 
		WHERE idMessage = @idMessage);

	DELETE FROM messages 
	WHERE idMessage = @idMessage;

	IF (
		SELECT COUNT(*) 
		FROM messages 
		WHERE @idChat = idChat) = 0
	DELETE FROM chats 
	WHERE idChat = @idChat;
END
GO
/****** Object:  StoredProcedure [dbo].[SendInventation]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SendInventation]

	@idUser int,
	@idOpponent int

AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO relationships (person1, person2, inventation) 
	VALUES (@idUser, @idOpponent, @idUser);
END
GO
/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SendMessage]
	
	@chatId int,
	@senderId int,
	@text nvarchar(1500),
	@date datetime,
	@id int OUTPUT  
	
AS
BEGIN
	SET NOCOUNT OFF;
	
	UPDATE chats
	SET unreaded = 
	CASE
		WHEN idPerson1 = @senderId
		THEN idPerson2
		WHEN idPerson2 = @senderId
		THEN idPerson1
	END
	WHERE idChat = @chatId;

	INSERT INTO messages (idChat, idSender, date, text) 
	VALUES (@chatId, @senderId, @date, @text);
	SELECT @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[SetPasswordById]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetPasswordById]

	@id int,
	@password nvarchar(500)

AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE users 
	SET password = @password 
	WHERE id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 07.10.2020 22:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]

	@id nchar(36),
	@name nvarchar(150),
	@birth date,
	@emblem nvarchar(250) = NULL,
	@statement nvarchar(250) = NULL,
	@blockedBy int = NULL


AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE users 
	SET name = @name, birth = @birth, statement = @statement, emblem = @emblem, blockedBy = @blockedBy
	WHERE id = @id;
END
GO
USE [master]
GO
ALTER DATABASE [SocialNetwork] SET  READ_WRITE 
GO
INSERT INTO [SocialNetwork].[dbo].[rolenames] (id, name) VALUES (1, 'admin');
GO
INSERT INTO [SocialNetwork].[dbo].[rolenames] (id, name) VALUES (2, 'user');
GO
INSERT INTO [SocialNetwork].[dbo].[users] (email, password, name, birth, statement, emblem, blockedBy)
VALUES ('admin@mail.ru', '25F43B1486AD95A1398E3EEB3D83BC4010015FCC9BEDB35B432E00298D5021F7', 'Администратор', '1989-10-07', 'Закон и порядок', '/avatars/63096865-1cf9-4d7a-a5b8-400374ba3030.png', NULL);
GO
INSERT INTO [SocialNetwork].[dbo].[users] (email, password, name, birth, statement, emblem, blockedBy)
VALUES ('admin2@mail.ru', '1C142B2D01AA34E9A36BDE480645A57FD69E14155DACFAB5A3F9257B77FDC8D8', 'Василий Петрович', '1998-11-08', 'Кто рано встаёт - тот молодец', '/avatars/472fb9c2-d89a-4bb2-9408-00f23ed45260.png', NULL);
GO
INSERT INTO [SocialNetwork].[dbo].[users] (email, password, name, birth, statement, emblem, blockedBy)
VALUES ('admin3@mail.ru', '4FC2B5673A201AD9B1FC03DCB346E1BAAD44351DAA0503D5534B4DFDCC4332E0', 'Андрей Михайлович', '1980-10-13', NULL, NULL, NULL);
GO
INSERT INTO [SocialNetwork].[dbo].[roles] (idUser, role) VALUES (1, 1);
GO
INSERT INTO [SocialNetwork].[dbo].[roles] (idUser, role) VALUES (1, 2);
GO
INSERT INTO [SocialNetwork].[dbo].[roles] (idUser, role) VALUES (2, 2);
GO
INSERT INTO [SocialNetwork].[dbo].[roles] (idUser, role) VALUES (3, 2);
GO
INSERT INTO [SocialNetwork].[dbo].[relationships] (person1, person2) VALUES (1, 2);
GO
INSERT INTO [SocialNetwork].[dbo].[relationships] (person1, person2) VALUES (1, 3);
GO