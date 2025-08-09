# ASP .Net Core Web API Training

## Table of Contents
- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Steps](#steps)

## Introduction
This is just a training project to play with DotNet Core API. All packages except for the SDK is installed within.
## Prerequisites
- .NET 8
- SQL Server
- MySql

## Steps: Cloning the Repository
1. Copy the project url from Github 
2. Open the terminal and type the command below to clone the repository
``` sh 
    Command: git clone <Clone using the web URL>
```
3. Open the project folder using VS Code 
4. Make sure to install .NET 9

## Steps: Creating the Database in SQLite3
1. Navigate to ASPNetCoreWebAPITraining/ASPNetCoreWebAPITraining


2. Open the appsettings.json and appsettings.Development.json files and replace <empty> for both files with the SQLite3 database path. You may need to put in different details depending on the database used.

``` sh
"ConnectionStrings": {
    "SQLite3": "Data Source=C:\\Users\\Databases\\DatabaseName.db;"
  },
```

3. Create migration files for desired database
``` sh 
Syntax:
dotnet ef migrations add Initial --context SQLite3DbContext --output-dir SQLite3Migrations
```

4. Update the database using the command 
``` sh 
Syntax:
dotnet ef database update --context SQLite3DbContext
```

* We're using the --context to specify which context provider to use in order to apply the migration.
* When we ran the command from step # 4, it creates and persists the database to MySQL. 


## Steps: Creating the Database in SQL Server
1. Update the database using the command 
``` sh 
Syntax:
dotnet ef database update --context SqlServerDbContext
```
