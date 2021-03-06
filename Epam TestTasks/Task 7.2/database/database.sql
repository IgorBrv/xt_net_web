USE [master]
GO
/****** Object:  Database [UsersAndAwards]    Script Date: 20.09.2020 21:36:56 ******/
CREATE DATABASE [UsersAndAwards]
 CONTAINMENT = NONE
GO
ALTER DATABASE [UsersAndAwards] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UsersAndAwards].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UsersAndAwards] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UsersAndAwards] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UsersAndAwards] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UsersAndAwards] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UsersAndAwards] SET ARITHABORT OFF 
GO
ALTER DATABASE [UsersAndAwards] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [UsersAndAwards] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UsersAndAwards] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UsersAndAwards] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UsersAndAwards] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UsersAndAwards] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UsersAndAwards] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UsersAndAwards] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UsersAndAwards] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UsersAndAwards] SET  DISABLE_BROKER 
GO
ALTER DATABASE [UsersAndAwards] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UsersAndAwards] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UsersAndAwards] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UsersAndAwards] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UsersAndAwards] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UsersAndAwards] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UsersAndAwards] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UsersAndAwards] SET RECOVERY FULL 
GO
ALTER DATABASE [UsersAndAwards] SET  MULTI_USER 
GO
ALTER DATABASE [UsersAndAwards] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UsersAndAwards] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UsersAndAwards] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UsersAndAwards] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [UsersAndAwards] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'UsersAndAwards', N'ON'
GO
ALTER DATABASE [UsersAndAwards] SET QUERY_STORE = OFF
GO
USE [UsersAndAwards]
GO
/****** Object:  Table [dbo].[awards]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[awards](
	[id] [nchar](36) NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[emblempath] [nvarchar](250) NULL,
 CONSTRAINT [PK_awards] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[persons]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[persons](
	[id] [nchar](36) NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[age] [int] NOT NULL,
	[birth] [date] NOT NULL,
	[emblempath] [nvarchar](250) NULL,
 CONSTRAINT [PK_persons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personsAwards]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personsAwards](
	[personid] [nchar](36) NOT NULL,
	[awardid] [nchar](36) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[name] [nvarchar](50) NOT NULL,
	[role] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_roles] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[name] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddAward]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddAward]

	@id nchar(36),
	@name nvarchar(150),
	@emblempath nvarchar(250) = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	
	INSERT INTO awards(id, name, emblempath) VALUES (@id, @name, @emblempath);
END
GO
/****** Object:  StoredProcedure [dbo].[AddAwardToUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddAwardToUser]

	@personid nchar(36),
	@awardid nchar(36)

AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO personsAwards(personid, awardid) VALUES (@personid, @awardid);
END
GO
/****** Object:  StoredProcedure [dbo].[AddPerson]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddPerson]

	@id nchar(36),
	@name nvarchar(150),
	@age int,
	@birth date,
	@emblempath nvarchar(250) = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	
	INSERT INTO persons (id, name, age, birth, emblempath) VALUES (@id, @name, @age, @birth, @emblempath)
END
GO
/****** Object:  StoredProcedure [dbo].[AwardsNumAtStorage]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AwardsNumAtStorage]

	@id nchar(36)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT COUNT(id) FROM awards WHERE id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[CreateRoleForUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateRoleForUser]

	@name nvarchar(50),
	@role nvarchar(50)

AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO roles(name, role) VALUES (@name, @role);
END
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUser]

	@name nvarchar(50),
	@password nvarchar(50)

AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO users (name, password) VALUES (@name, @password);
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllAwards]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllAwards]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM awards;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllAwardsOfUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllAwardsOfUser]
	
	@id nchar(36)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT awardid FROM personsAwards WHERE personid = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllPersons]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllPersons]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT * FROM persons
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllPersonsWithAward]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllPersonsWithAward]
	
	@id nchar(36)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT personid FROM personsAwards WHERE awardid = @id;
END

GO
/****** Object:  StoredProcedure [dbo].[GetAward]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAward]
	@id nchar(36)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT id, name, emblempath FROM awards WHERE id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetListOfAwarded]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetListOfAwarded]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM personsAwards
END
GO
/****** Object:  StoredProcedure [dbo].[GetNumOfUsersRoles]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNumOfUsersRoles]

	@name nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(name) FROM roles WHERE name = @name;
END
GO
/****** Object:  StoredProcedure [dbo].[GetPassword]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPassword]
	
	@name nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT password FROM users WHERE name = @name;
END
GO
/****** Object:  StoredProcedure [dbo].[GetPerson]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPerson]
	@id nchar(36)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT id, name, age, birth, emblempath FROM persons WHERE id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleOfUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleOfUser]

	@name nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT role FROM roles WHERE name = @name;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserCount]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserCount]
	
	@name nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(name) FROM users WHERE name = @name;
END
GO
/****** Object:  StoredProcedure [dbo].[PersonsNumAtStorage]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PersonsNumAtStorage]

	@id nchar(36)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT COUNT(id) FROM persons WHERE id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveAward]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveAward]

	@id nchar(36)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE FROM awards WHERE id = @id;
	DELETE FROM personsAwards WHERE awardid = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveAwardFromUser]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveAwardFromUser]

	@personid nchar(36),
	@awardid nchar(36)

AS
BEGIN
	SET NOCOUNT OFF;
	
	DELETE FROM personsAwards WHERE personid = @personid and awardid = @awardid;
END
GO
/****** Object:  StoredProcedure [dbo].[RemovePerson]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemovePerson]

	@id nchar(36)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE FROM persons WHERE id = @id;
	DELETE FROM personsAwards WHERE personid = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAward]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateAward]

	@id nchar(36),
	@name nvarchar(150),
	@emblempath nvarchar(250) = NULL

AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE awards SET name = @name, emblempath = @emblempath WHERE id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePerson]    Script Date: 20.09.2020 21:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePerson]
	-- Add the parameters for the stored procedure here
	@id nchar(36),
	@name nvarchar(150),
	@age int,
	@birth date,
	@emblempath nvarchar(250) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
	UPDATE persons SET name = @name, age = @age, birth = @birth, emblempath = @emblempath
	WHERE id = @id;
END
GO
USE [master]
GO
ALTER DATABASE [UsersAndAwards] SET  READ_WRITE 
GO
INSERT INTO [UsersAndAwards].[dbo].[roles](name, role) VALUES ('admin', 'admin')
GO
INSERT INTO [UsersAndAwards].[dbo].[roles](name, role) VALUES ('user', 'user')
GO
INSERT INTO [UsersAndAwards].[dbo].[users](name, password) VALUES ('admin', 'admin')
GO
INSERT INTO [UsersAndAwards].[dbo].[users](name, password) VALUES ('user', 'user')
GO
