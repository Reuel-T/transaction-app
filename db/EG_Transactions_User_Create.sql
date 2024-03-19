USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [transaction]    Script Date: 2024/03/19 10:34:00 ******/
CREATE LOGIN [transaction] WITH PASSWORD='trans', DEFAULT_DATABASE=[EG_Transactions], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

/*ALTER LOGIN [transaction] DISABLE
GO*/

USE [EG_Transactions]
GO

/****** Object:  User [trans]    Script Date: 2024/03/19 10:34:59 ******/
CREATE USER [trans] FOR LOGIN [transaction] WITH DEFAULT_SCHEMA=[dbo]
GO

