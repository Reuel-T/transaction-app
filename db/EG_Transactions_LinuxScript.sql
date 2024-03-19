USE [master]
GO
/****** Object:  Database [EG_Transactions]    Script Date: 2024/03/19 09:44:59 ******/
CREATE DATABASE [EG_Transactions]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EG_Transactions', FILENAME = N'/var/opt/mssql/data/EG_Transactions.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EG_Transactions_log', FILENAME = N'/var/opt/mssql/data/EG_Transactions_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EG_Transactions] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EG_Transactions].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EG_Transactions] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EG_Transactions] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EG_Transactions] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EG_Transactions] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EG_Transactions] SET ARITHABORT OFF 
GO
ALTER DATABASE [EG_Transactions] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EG_Transactions] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EG_Transactions] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EG_Transactions] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EG_Transactions] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EG_Transactions] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EG_Transactions] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EG_Transactions] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EG_Transactions] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EG_Transactions] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EG_Transactions] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EG_Transactions] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EG_Transactions] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EG_Transactions] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EG_Transactions] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EG_Transactions] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EG_Transactions] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EG_Transactions] SET RECOVERY FULL 
GO
ALTER DATABASE [EG_Transactions] SET  MULTI_USER 
GO
ALTER DATABASE [EG_Transactions] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EG_Transactions] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EG_Transactions] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EG_Transactions] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EG_Transactions] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EG_Transactions] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EG_Transactions', N'ON'
GO
ALTER DATABASE [EG_Transactions] SET QUERY_STORE = ON
GO
ALTER DATABASE [EG_Transactions] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EG_Transactions]
GO
/****** Object:  User [trans]    Script Date: 2024/03/19 09:44:59 ******/
CREATE USER [trans] FOR LOGIN [transaction] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [trans]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [trans]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [trans]
GO
ALTER ROLE [db_datareader] ADD MEMBER [trans]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [trans]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[ClientBalance] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Comment] [nvarchar](100) NULL,
	[TransactionTypeID] [smallint] NULL,
	[ClientID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[TransactionTypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[TransactionTypeName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Client] ON 
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (2, N'Tony', N'Stark', CAST(1000000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (3, N'Bruce', N'Banner', CAST(257601.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (4, N'Richard', N'Grayson', CAST(200000000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (5, N'Barbara', N'Gordon', CAST(100000000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (17, N'Garfield', N'Logan', CAST(-2600.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (18, N'Donna', N'Troy', CAST(2070.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (19, N'Barry', N'Allen', CAST(8800.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (20, N'Victor', N'Stone', CAST(295350.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Client] ([ClientID], [Name], [Surname], [ClientBalance]) VALUES (21, N'Kory', N'Anders', CAST(-891900.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (3, CAST(-500.00 AS Decimal(18, 2)), N'TEST', 2, 3)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (4, CAST(-9000.00 AS Decimal(18, 2)), N'Loss', 2, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (8, CAST(200000.00 AS Decimal(18, 2)), N'Winnings', 1, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (9, CAST(100000000.00 AS Decimal(18, 2)), N'Winnings', 1, 4)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (10, CAST(3000.00 AS Decimal(18, 2)), N'Winnings', 1, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (11, CAST(-3000.00 AS Decimal(18, 2)), N'Loss', 2, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (12, CAST(1234.00 AS Decimal(18, 2)), N'Winnings', 1, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (13, CAST(-1234.00 AS Decimal(18, 2)), N'Loss', 2, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (14, CAST(2000000.00 AS Decimal(18, 2)), N'Winning', 1, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (15, CAST(-1000000.00 AS Decimal(18, 2)), N'Loss', 2, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (16, CAST(-1000000.00 AS Decimal(18, 2)), N'Loss', 2, 2)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (17, CAST(1000.00 AS Decimal(18, 2)), N'Winnings', 1, 17)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (18, CAST(350.00 AS Decimal(18, 2)), N'Winnings', 1, 5)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (19, CAST(350.00 AS Decimal(18, 2)), N'Winnings', 1, 5)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (20, CAST(-4800.00 AS Decimal(18, 2)), N'Loss', 2, 5)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (21, CAST(-4800.00 AS Decimal(18, 2)), N'Loss', 2, 5)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (22, CAST(800.00 AS Decimal(18, 2)), N'Winnings', 1, 5)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (23, CAST(-4750.00 AS Decimal(18, 2)), N'Loss', 2, 20)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (24, CAST(-600.00 AS Decimal(18, 2)), N'Loss', 2, 20)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (25, CAST(875.00 AS Decimal(18, 2)), N'Winnings', 1, 20)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (26, CAST(-500.00 AS Decimal(18, 2)), N'Loss', 2, 20)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (27, CAST(325.00 AS Decimal(18, 2)), N'Winnings', 1, 20)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (28, CAST(800.00 AS Decimal(18, 2)), N'Winnings', 1, 21)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (29, CAST(970.00 AS Decimal(18, 2)), N'Winnings', 1, 21)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (30, CAST(-260.00 AS Decimal(18, 2)), N'Loss', 2, 21)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (31, CAST(5000.00 AS Decimal(18, 2)), N'Winnings', 1, 21)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (32, CAST(1590.00 AS Decimal(18, 2)), N'Loss', 1, 21)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (33, CAST(-600.00 AS Decimal(18, 2)), N'Loss', 2, 17)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (34, CAST(1790.00 AS Decimal(18, 2)), N'Winnings', 1, 18)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (35, CAST(-600.00 AS Decimal(18, 2)), N'Loss', 2, 18)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (36, CAST(500.00 AS Decimal(18, 2)), N'Winnings', 1, 18)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (37, CAST(320.00 AS Decimal(18, 2)), N'Winnings', 1, 18)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (38, CAST(-840.00 AS Decimal(18, 2)), N'Loss', 2, 18)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (39, CAST(6000.00 AS Decimal(18, 2)), N'Winnings', 1, 3)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (40, CAST(-2500.00 AS Decimal(18, 2)), N'Loss', 2, 3)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (41, CAST(450.00 AS Decimal(18, 2)), N'Loss', 1, 3)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (42, CAST(-460.00 AS Decimal(18, 2)), N'Loss', 2, 3)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (43, CAST(8000.00 AS Decimal(18, 2)), N'Winnings', 1, 19)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (44, CAST(-2800.00 AS Decimal(18, 2)), N'Loss', 2, 19)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (45, CAST(-4500.00 AS Decimal(18, 2)), N'Loss', 2, 19)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (46, CAST(-4500.00 AS Decimal(18, 2)), N'Loss', 2, 19)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (47, CAST(2600.00 AS Decimal(18, 2)), N'Winnings', 1, 19)
GO
INSERT [dbo].[Transaction] ([TransactionID], [Amount], [Comment], [TransactionTypeID], [ClientID]) VALUES (48, CAST(8100.00 AS Decimal(18, 2)), N'Winnings', 1, 5)
GO
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionType] ON 
GO
INSERT [dbo].[TransactionType] ([TransactionTypeID], [TransactionTypeName]) VALUES (1, N'Debit')
GO
INSERT [dbo].[TransactionType] ([TransactionTypeID], [TransactionTypeName]) VALUES (2, N'Credit')
GO
SET IDENTITY_INSERT [dbo].[TransactionType] OFF
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([TransactionTypeID])
REFERENCES [dbo].[TransactionType] ([TransactionTypeID])
GO
/****** Object:  StoredProcedure [dbo].[SPCreateTransaction]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPCreateTransaction]
	@Amount DECIMAL(18,2),
	@Comment NVARCHAR(100),
	@TransactionTypeID SMALLINT,
	@ClientID INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION;
		--Transaction and Save the new ID
		INSERT INTO [Transaction] (Amount, Comment, TransactionTypeID, ClientID)
		VALUES (@Amount, @Comment, @TransactionTypeID, @ClientID);

		DECLARE @NewTransactionID BIGINT;
		SET @NewTransactionID = SCOPE_IDENTITY();

		--Update the Client's balance
		UPDATE Client
		SET ClientBalance = ClientBalance + @Amount
		WHERE ClientID = @ClientID;

		--commit transaction
		COMMIT;

		SELECT @NewTransactionID
	END TRY
	BEGIN CATCH
		--rollback if error happens
		ROLLBACK;
		--Throw to client?
		THROW;
	END CATCH
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[SPDeleteClientAndTransactions]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPDeleteClientAndTransactions]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION;
			--First Delete Transactions for client
			DELETE FROM [Transaction]
			WHERE ClientID = @Id;

			--save the number of transactions removed
			DECLARE @TransactionRowsAffected INT;
			SET @TransactionRowsAffected = @@ROWCOUNT;

			--Delete the Client
			DELETE FROM Client
			WHERE ClientID = @Id;

			--Save the number of clients removed
			DECLARE @ClientRowsAffected INT;
			SET @ClientRowsAffected = @@ROWCOUNT;

			--commit
			COMMIT;
			--return the values
			SELECT @ClientRowsAffected AS ClientsRemoved, @TransactionRowsAffected AS TransactionsRemoved
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[SPGetTransactionsForClient]    Script Date: 2024/03/19 09:45:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPGetTransactionsForClient]
	@ClientID INT
AS
BEGIN
	SELECT T.[TransactionID], T.[Amount], TT.[TransactionTypeName], T.[ClientID], T.[Comment]
    FROM [Transaction] T
    JOIN [TransactionType] TT ON T.TransactionTypeID = TT.TransactionTypeID
    WHERE T.[ClientID] = @ClientID;
END	
GO
USE [master]
GO
ALTER DATABASE [EG_Transactions] SET  READ_WRITE 
GO
