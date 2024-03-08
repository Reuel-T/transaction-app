CREATE DATABASE EG_Transactions

CREATE TABLE TransactionType(
	TransactionTypeID SMALLINT IDENTITY PRIMARY KEY,
	TransactionTypeName NVARCHAR(50) NOT NULL
)

CREATE TABLE Client(
	ClientID INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL,
	Surname NVARCHAR(50) NOT NULL,
	ClientBalance DECIMAL(18,2) NOT NULL
)

CREATE TABLE [Transaction](
	TransactionID BIGINT IDENTITY PRIMARY KEY,
	Amount DECIMAL(18,2) NOT NULL,
	Comment NVARCHAR(100),

	TransactionTypeID SMALLINT FOREIGN KEY REFERENCES TransactionType(TransactionTypeID),
	ClientID INT FOREIGN KEY REFERENCES Client(ClientID)
)


SELECT * FROM TransactionType
SELECT * FROM Client
SELECT * FROM [Transaction]

UPDATE Client
    SET ClientBalance = ClientBalance + 300
    WHERE ClientID = 16;
SELECT ClientBal

INSERT INTO TransactionType (TransactionTypeName) VALUES ('Debit');
INSERT INTO TransactionType (TransactionTypeName) VALUES ('Credit');

INSERT INTO Client (Name, Surname, ClientBalance)
VALUES 
    ('Peter', 'Parker', 1000.00),
    ('Tony', 'Stark', 800000),
    ('Bruce', 'Banner', 254111);

INSERT INTO Client (Name, Surname, ClientBalance)
VALUES 
    ('Bruce', 'Wayne', 50000000.00)
SELECT CAST(SCOPE_IDENTITY() as int)

INSERT INTO [Transaction] (Amount, Comment, TransactionTypeID, ClientID)
VALUES 
    (1000.00, 'Winnings', 1, 1),
    (-500, 'Loss', 2, 3),
    (-9000, 'Loss', 2, 2);


SELECT T.TransactionID, C.[Name], C.[Surname], T.[Amount], TT.[TransactionTypeName] FROM Client C
JOIN [Transaction] T ON C.ClientID = T.ClientID
JOIN [TransactionType] TT ON T.TransactionTypeID = TT.TransactionTypeID

SELECT T.[TransactionID], T.[Amount], TT.[TransactionTypeName], T.[ClientID] FROM [Transaction] T
JOIN [TransactionType] TT ON T.TransactionTypeID = TT.TransactionTypeID
WHERE T.[ClientID] = 3
 


CREATE PROCEDURE SPCreateTransaction
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

EXEC SPCreateTransaction
	@Amount = 7000,
	@Comment = 'Winnings',
	@TransactionTypeID = 1,
	@ClientID = 16


CREATE PROCEDURE SPDeleteClientAndTransactions
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

EXEC SPDeleteClientAndTransactions @Id = 16;
EXEC SPDeleteClientAndTransactions @Id = 9;

DROP PROCEDURE SPDeleteClientAndTransactions

CREATE PROCEDURE SPGetTransactionsForClient
	@ClientID INT
AS
BEGIN
	SELECT T.[TransactionID], T.[Amount], TT.[TransactionTypeName], T.[ClientID], T.[Comment]
    FROM [Transaction] T
    JOIN [TransactionType] TT ON T.TransactionTypeID = TT.TransactionTypeID
    WHERE T.[ClientID] = @ClientID;
END		

EXEC SPGetTransactionsForClient @ClientID = 3

DROP PROCEDURE SPGetTransactionsForClient