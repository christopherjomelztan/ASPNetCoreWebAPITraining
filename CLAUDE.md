# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A training project: a single ASP.NET Core Web API (`net10.0`, EF Core 10) exposing CRUD for one `Person` entity, backed by EF Core against a **runtime-selectable database provider** (SQLite3, SQL Server, Microsoft Access).

The API lives in the nested `ASPNetCoreWebAPITraining/` folder and its xUnit tests in the sibling `ASPNetCoreWebAPITraining.Tests/`. The repo root `.sln` holds both. Run `dotnet build`/`dotnet run`/`dotnet ef` from the nested API folder, and `dotnet test` from the repo root.

MySQL support was dropped when moving to EF Core 10: Pomelo has no EF 10 release, and its 9.0.0 pins EF Relational to 9.0.x. Restore it from git history if Pomelo ships one.

## Commands

Run from `ASPNetCoreWebAPITraining/ASPNetCoreWebAPITraining/` (the folder containing the API `.csproj`) unless noted.

```sh
dotnet tool restore          # installs dotnet-ef 10.0.13 from .config/dotnet-tools.json
dotnet build
dotnet run --launch-profile https   # Swagger UI at https://localhost:7273/swagger (http profile: http://localhost:5288)
```

Tests (from the repo root):

```sh
dotnet test
dotnet test --filter "FullyQualifiedName~MicrosoftAccessPersonApiTests"   # one provider
dotnet test --filter "FullyQualifiedName~PersonApiContractTests.GetPerson_WhenMissing_ReturnsNotFound"   # one test, all providers
```

The API contract tests (`Api/PersonApiContractTests`) run once per provider (SQL Server LocalDB, Access, SQLite) against the real API through `WebApplicationFactory`. Each provider's factory creates a uniquely named throwaway database from the migrations (so seed data is exercised too) and drops it afterwards. They need LocalDB (`(localdb)\MSSQLLocalDB`) and the 64-bit ACE OLE DB driver installed. Add a new provider's tests by adding a `ProviderApiFactory` subclass and a thin test subclass.

EF migrations are per provider, so every `dotnet ef` command needs `--context` (and `--output-dir` when adding):

```sh
dotnet ef migrations add <Name> --context SQLite3DbContext --output-dir SQLite3Migrations
dotnet ef database update --context SQLite3DbContext
```

Swap `SQLite3DbContext`/`SQLite3Migrations` for `SqlServerDbContext`/`SqlServerMigrations` or `MicrosoftAccessDbContext`/`MicrosoftAccessMigrations`. The `ASPNetCoreWebAPITraining.http` file has sample requests for the endpoints.

`System.Security.Cryptography.Pkcs` is a direct reference only to override the vulnerable 9.0.11 that the latest `Microsoft.Data.SqlClient` (via `Microsoft.EntityFrameworkCore.SqlServer`) pulls in. Remove the pin once SqlClient ships a fixed version (`dotnet list package --vulnerable --include-transitive` to check).

## Configuration

- `appsettings.json` holds a working connection string per provider under `ConnectionStrings`, keyed by the constants in `StaticConfiguration.cs`: SQL Server LocalDB (`TopeTraining`), an Access `TopeTraining.accdb` via `Microsoft.ACE.OLEDB.12.0`, and SQLite `TopeTrainingSQLite3.db`. File databases are created in the project folder and git-ignored. The Access provider is Windows-only and needs the ACE drivers matching the .NET process bitness.
- The top-level `Provider` setting (`SqlServer` | `SQLite3` | `MicrosoftAccess`) picks which database the API actually uses. both `appsettings.json` and `appsettings.Development.json` set it to `MicrosoftAccess`. Override ad hoc with `--Provider <value>`.
- Swagger/Swagger UI are only enabled when `ASPNETCORE_ENVIRONMENT=Development` (set by every launch profile).

## Architecture

The non-obvious part is how the provider switch works across several files:

1. `Database/DatabaseServiceCollectionExtensions.AddPersonDatabase` (called from `Program.cs`) registers **all three** `DbContext`s with `AddDbContext` (needed so `dotnet ef --context` can find each), then registers a scoped `BaseDbContext` that resolves to the context of the `Provider` setting. An unrecognised provider throws at startup.
2. `Database/BaseDbContext` holds the shared `DbSet<Person> Persons` and seeds `Database/SeedData.cs` through `HasData`. Each provider has a thin subclass (`SqlServerDbContext`, `SQLite3DbContext`, `MicrosoftAccessDbContext`) that only forwards its typed options.
3. `Controllers/PersonController` injects `BaseDbContext` and so never knows which provider is behind it.
4. The `*Migrations/` folders contain one migration history per provider. A model or seed-data change needs a new migration for each provider you intend to support (seed rows are applied by migration, not at startup).

Adding a provider means: a `DbContext` subclass, a `StaticConfiguration` constant, a connection string key, and an `AddDbContext` line plus a `switch` arm in `AddPersonDatabase`. Add its migrations and a `ProviderApiFactory` + test subclass too.
