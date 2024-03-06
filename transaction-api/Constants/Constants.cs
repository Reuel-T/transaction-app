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
            public const string ClientID = "ClientID";
            public const string Name = "Name";
            public const string Surname = "Surname";
            public const string ClientBalance = "ClientBalance";
        }

    }
}
