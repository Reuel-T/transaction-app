# Transaction App

![FrontEnd Screenshot](https://github.com/Reuel-T/transaction-app/assets/69512501/b8e6302f-073b-4018-92d1-15db00a50a64)

This App consists of three parts:
- [A Vue3 Frontend](https://vuejs.org/)
- [.NET 8 Web API](https://dotnet.microsoft.com/en-us/)
- [SQL Server Database](https://www.microsoft.com/en-za/sql-server/sql-server-downloads)

This is also set up to run the API and Database in Docker if you wish to do so

### Setup

#### Database

A script to create the database schema and data within is included. Extra Linux based script included for SQL Server running in Linux Containers.

To run the docker containers use the following command in the project root
```bash
docker compose up -d
```

To set up the database in the container, you will need to login in using your DBMS of choice (SSMS or 
Azure Data Studio recommended)

The database in the container uses port `3341`.
You need to use the `sa` login to access it initially as that is the only way in

``` 
    Username: sa
    Password: YourStrong@Passw0rd
```


```
.
├── client
├── transaction-api
└── db/
    └── EG_Transactions.sql
    └── EG_Transactions_Linux.sql
    └── EG_Transactions_User_Create.sql
```

An untested script has been included in `db` to create the login and user for the `EG_Transactions` database. If this does not work, you should create your own user and modify the connection string.

#### API

Set the connection string. Place the connection string of your newly created database in `appsettings.json` If running the containerised version, you should use the DockerConnection string instead, as 
an environment variable is set in the container version to use the Docker one instead.

```
.
├── client
├── db
└── transaction-api/
    └── appsettings.json
```

```json
"ConnectionStrings": {
    "DefaultConnection": "YOUR CONNECTION STRING GOES HERE",
    "DockerConnection" : "DOCKER CONNECTION STRING"
  }
```

This requires .NET 8. This project was created using Visual Studio so you can open up the solution and run it there, but running

```bash
dotnet run
```

in the project directory should suffice.

```
.
├── client
├── db
└── transaction-api/
    └── transaction-api.sln
```

The API is configured to listen on :https://localhost:7333 (This might require the acceptance of developer certificates)

Once successfully running, you can visit https://localhost:7333/swagger/index.html to have a look at and play with the routes in the API for testing purposes without having to run any client application if you wish. On the containerised version visit http://localhost:5333/swagger/index.html

Running the API in Docker (through docker compose) uses http://localhost:5333 for the API and 

#### Client

```
.
├── client/
│   └── package.json
├── db
└── transaction-api
```

To run the client navigate to the client folder and run

```bash
npm i
```
Followed by:
```bash
npm run dev
```

The client runs on http://localhost:5173/ 
