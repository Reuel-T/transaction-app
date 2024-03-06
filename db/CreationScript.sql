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

INSERT INTO TransactionType (TransactionTypeName) VALUES ('Debit');
INSERT INTO TransactionType (TransactionTypeName) VALUES ('Credit');

INSERT INTO Client (Name, Surname, ClientBalance)
VALUES 
    ('Peter', 'Parker', 1000.00),
    ('Tony', 'Stark', 800000),
    ('Bruce', 'Banner', 254111);



INSERT INTO [Transaction] (Amount, Comment, TransactionTypeID, ClientID)
VALUES 
    (1000.00, 'Winnings', 1, 1),
    (-500, 'Loss', 2, 3),
    (-9000, 'Loss', 2, 2);


SELECT T.TransactionID AS 'Transaction ID', [Name], Surname, Amount FROM Client C
JOIN [Transaction] T ON C.ClientID = T.ClientID



