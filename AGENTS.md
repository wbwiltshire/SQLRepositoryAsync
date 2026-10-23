# SQLRepositoryAsync Agent Instructions

## Project

- Reference C# / .NET repository-pattern implementation using async ADO.NET and `Microsoft.Data.SqlClient`.
- Solution projects: [Data](Data), [POCO](POCO), [Repository](Repository), and [Regression.Test](Regression.Test).
- Public behavior and API intent are described in [README.md](README.md). Treat it as useful context, not a substitute for checking current async signatures.

## Architecture

- `Data` owns interfaces and base contracts.
- `POCO` owns entity models.
- `Repository` owns SQL access, mappings, paging, and unit-of-work behavior.
- `Regression.Test` owns SQL Server integration tests and the database fixture.
- Preserve the dependency flow: `Regression.Test` may reference `Repository`, `POCO`, and `Data`; `Repository` references `POCO` and `Data`; `POCO` references `Data`.
- Repository implementations normally inherit `RepositoryBase<TEntity>` and implement `IRepository<TEntity>`.
- Preserve primary-key behavior in `MapperBase` and `PrimaryKey`: identity, natural, and composite keys produce the repository's existing parameter names and SQL shape.
- Keep paging and sorting coupled through `IPager`; sorted-column names are allowlisted by `RepositoryBase` rather than interpolated directly.
- Keep direct SQL and stored-procedure branches aligned. Procedure names come from [appsettings.json](Regression.Test/appsettings.json), and repository SQL parameter names must match [CreateStoredProcedures.sql](SQL/CreateStoredProcedures.sql).
- Deletes are soft deletes: set `Active = 0`, and normal reads exclude inactive rows.

## Commands

Run from the repository root:

```powershell
dotnet restore .\SQLRepositoryAsync.sln
dotnet build .\SQLRepositoryAsync.sln -c Debug
dotnet test .\Regression.Test\Regression.Test.csproj -c Debug --no-build
```

Use `dotnet test` to run a focused class or test:

```powershell
dotnet test .\Regression.Test\Regression.Test.csproj -c Debug --filter FullyQualifiedName~Regression.Test.QueryTests
dotnet test .\Regression.Test\Regression.Test.csproj -c Debug --filter FullyQualifiedName=Regression.Test.TransactionTests.SaveTest
```

There is no global SDK or repository-wide formatter/analyzer policy. Do not change target frameworks or dependency versions unless the task explicitly requires it.

## Database and Tests

- Tests require a reachable SQL Server, credentials, `DCustomer`, the expected backup seed data, views, and stored procedures.
- Run [ReloadDatabase.cmd](SQL/ReloadDatabase.cmd) from the `SQL` directory before a transaction-test rerun or after changing seed-dependent state. Read the script before running it because it destructively recreates tables and data.
- The reload script recreates tables and loads seed data, but the procedure and view creation scripts are separate.
- [appsettings.json](Regression.Test/appsettings.json) is not copied to test output; test discovery or startup can fail when run with only its committed location. Do not assume every test run has a working database configuration.
- `QueryTests` and `TransactionTests` share one xUnit collection fixture. Avoid adding parallel stateful tests or changing the fixture to hide live database dependencies.
- Transaction tests mutate live seed records and can make later runs fail unless the database is reloaded first.
- Update [TestMatrix.md](Regression.Test/TestMatrix.md) when adding or removing repository behavior coverage.
- [RunRegressionTest.ps1](RunRegressionTest.ps1) runs the net10.0 test assembly. Prefer direct `dotnet test` commands.

## SQL and Safety

- SQL scripts use destructive `DROP`/create/rebuild patterns. Never run them unless the requested task is database setup or explicit destruction.
- Preserve SQL batch structure and `GO` separators in existing scripts. Rebuild foreign keys after dependent tables.
- Keep bulk-load, database-name, backup-path, and SQLCMD/BCP assumptions aligned with the target database.
- Use parameterized SQL; do not concatenate untrusted values into command text.
- In ADO.NET, await reader/command execution and close commands/readers in their owning scope.
- The shared unit of work uses one managed `SqlTransaction` per instance and is not concurrent-safe. Do not introduce parallel access to one repository or unit of work instance.

## Editing Conventions

- Keep changes narrowly scoped to the owning project and behavior.
- Match existing C# namespaces, class names, public async Task contracts, and repository-specific SQL patterns.
- Add or update focused xUnit integration coverage for behavior changes.
- Do not add packages or change project targets without user confirmation.
- Verify changes with the smallest relevant build or test command; database-dependent failures should be reported separately from compile/test regressions.
- Leave generated `bin/`, `obj/`, and `TestResults/` artifacts untouched and untracked.
