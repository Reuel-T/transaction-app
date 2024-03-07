namespace transaction_api.Constants
{
    public  class Constants
    {
        public const string DefaultConnection = "DefaultConnection";

        public struct Tables
        {
            public const string Client = "[Client]";
            public const string TransactionType = "[TransactionType]";
            public const string Transaction = "[Transaction]";
        }

        public struct ClientFields
        {
            public const string ClientID = "[ClientID]";
            public const string Name = "[Name]";
            public const string Surname = "[Surname]";
            public const string ClientBalance = "[ClientBalance]";
        }

        public struct TransactionFields
        {
            public const string TransactionID = "[TransactionID]";
            public const string Amount = "[Amount]";
            public const string Comment = "[Comment]";
            public const string TransactionTypeID = "[TransactionTypeID]";
            public const string ClientID = "[ClientID]";
        }

        public struct TransactionTypeFields 
        {
            public const string TransactionTypeID = "[TransactionTypeId]";
            public const string TransactionTypeName = "[TransactionTypeName]";
        }

        public struct StoredProcedures 
        {
            public const string CreateTransaction = "SPCreateTransaction";
            public const string DeleteClient = "SPDeleteClientAndTransactions";
        }

    }
}
