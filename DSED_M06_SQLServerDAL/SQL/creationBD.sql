USE master;
GO

IF DB_ID('m06_comptes') IS NULL
	CREATE DATABASE m06_comptes;
GO

USE m06_comptes;
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE [name] = 'compte' AND xtype = 'U')
	CREATE TABLE compte (
		numeroCompte NVARCHAR(200) PRIMARY KEY,
		[type] NVARCHAR(200) NOT NULL,
		);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE [name] = 'transaction' AND xtype = 'U')
	CREATE TABLE [transaction] (
		transactionId NVARCHAR(200) PRIMARY KEY,
		[type] NVARCHAR(200) NOT NULL,
		[date] DATETIME NOT NULL,
		montant MONEY NOT NULL,
		numeroCompte NVARCHAR(200) CONSTRAINT FK_compte_numeroCompte FOREIGN KEY REFERENCES compte(numeroCompte),

		INDEX IX_transaction_numeroCompte NONCLUSTERED (numeroCompte),
		);
GO

SELECT * FROM compte;