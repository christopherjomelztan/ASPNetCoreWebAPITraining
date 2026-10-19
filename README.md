# ASP .Net Core Web API Training

## Table of Contents
- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Steps: Cloning the Repository](#steps-cloning-the-repository)
- [Steps: Creating the Database in SQLite3](#steps-creating-the-database-in-sqlite3)
- [Steps: Creating the Database in SQL Server](#steps-creating-the-database-in-sql-server)
- [Steps: Creating the Database in Microsoft Access](#steps-creating-the-database-in-microsoft-access)
- [Steps: Running the API](#steps-running-the-api)
- [Steps: Running the Tests](#steps-running-the-tests)

## Introduction
This is just a training project to play with DotNet Core API. It exposes CRUD endpoints for a `Person` entity (`api/Person`) over EF Core, and the database provider (SQLite3, SQL Server or Microsoft Access) is chosen at runtime with the `Provider` setting. All packages except for the SDK are installed within.

## Prerequisites
- .NET 10 SDK
- SQL Server or LocalDB (only if using the SQL Server provider)
- Microsoft Access Database Engine (ACE) 64-bit drivers (only if using the Microsoft Access provider, Windows only)

## Steps: Cloning the Repository
1. Copy the project url from Github
2. Open the terminal and type the command below to clone the repository
``` sh
    Command: git clone <Clone using the web URL>
```
3. Open the project folder using VS Code
4. Make sure to install .NET 10

## Steps: Creating the Database in SQLite3
1. Navigate to ASPNetCoreWebAPITraining/ASPNetCoreWebAPITraining and restore the local `dotnet-ef` tool

``` sh
dotnet tool restore
```

2. Optionally change the SQLite3 connection string in `appsettings.json`. By default it creates `TopeTrainingSQLite3.db` in the project folder (git-ignored).

``` sh
"ConnectionStrings": {
    "SQLite3": "Data Source=TopeTrainingSQLite3.db;"
  },
```

3. Run the migrations, which create the database and seed five `Person` rows.

``` sh
dotnet ef database update --context SQLite3DbContext
```

* We're using the --context to specify which context provider to use in order to apply the migration.
* The seed data lives in `Database/SeedData.cs`. After changing it (or the model), add a migration per provider:

``` sh
dotnet ef migrations add <Name> --context SQLite3DbContext --output-dir SQLite3Migrations
```

## Steps: Creating the Database in SQL Server
1. The `SqlServer` entry of `appsettings.json` targets SQL Server LocalDB (`(localdb)\MSSQLLocalDB`, database `TopeTraining`). Change it to point at another instance if needed.
2. Update the database using the command (creates the database, table and seed rows)
``` sh
dotnet ef database update --context SqlServerDbContext
```

## Steps: Creating the Database in Microsoft Access
1. Windows only. Needs the Microsoft Access Database Engine (ACE) OLE DB provider matching the .NET process bitness (64-bit).
2. The `MicrosoftAccess` entry of `appsettings.json` creates `TopeTraining.accdb` in the project folder (git-ignored).
3. Update the database using the command (creates the file, table and seed rows)
``` sh
dotnet ef database update --context MicrosoftAccessDbContext
```

## Steps: Running the API
``` sh
dotnet run --launch-profile https
```
The `Provider` setting picks the database and is `MicrosoftAccess` in both `appsettings.json` and `appsettings.Development.json`. Override it on the command line with e.g. `dotnet run --launch-profile https --Provider SQLite3`.
The API docs are rendered by Redoc at https://localhost:7273/redoc (or http://localhost:5288/redoc with the `http` profile), from the OpenAPI document at `/openapi/v1.json`. `ASPNetCoreWebAPITraining.http` contains sample requests.

## Steps: Running the Tests
From the repository root:
``` sh
dotnet test
```
The API tests run against SQL Server LocalDB, Microsoft Access and SQLite. Each uses a uniquely named throwaway database that is created from the migrations and dropped afterwards, so LocalDB and the 64-bit ACE drivers must be installed.

## Notes
- MySQL support was removed when the project moved to .NET 10 / EF Core 10, because the Pomelo MySQL provider has no EF Core 10 release yet. It can be restored from git history once one is published.
