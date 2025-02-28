# ASP .Net Core Web API Training

## Table of Contents
- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Steps](#steps)

## Introduction

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
4. Make sure to install .NET 8

## Steps: Creating the Database in MySQL
1. Open the appsettings.json and appsettings.Development.json files and replace <empty> for both files with the MySQL Root Password that you set during the installation of MySQL otherwise when you run the command that updates the database it will throw an error as shown below:
Access denied for user 'root'@'localhost' (using password: YES)

``` sh
"ConnectionStrings": {
    "MySql": "Server=localhost;User ID=root;Password=abcd;Database=Training;",
  },
```

2. Create migration files for desired database
``` sh 
Syntax:
dotnet ef migrations add Initial --context SQLite3DbContext --output-dir SQLite3Migrations
```

3. Update the database using the command 
``` sh 
Syntax:
dotnet ef database update --context MySqlDbContext
```

* We're using the --context to specify which context provider to use in order to apply the migration.
* When we ran the command from step # 4, it creates and persists the database to MySQL. 


## Steps: Creating the Database in SQL Server
1. Update the database using the command 
``` sh 
Syntax:
dotnet ef database update --context SqlServerDbContext
```