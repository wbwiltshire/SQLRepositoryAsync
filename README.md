# SQLRepositoryAsync

A reference application written in C# .Net Core which implements the Repository pattern and uses ADO.NET to asynchronously access a SQL Server database.
Regression testing of queries and transactions is performed using XUnit.

## Requirements

### Features
* Implements the Repository Pattern
* All methods execute asynchronously
* Supports Unit of Work
* Repository instances support concurrent read/write operations with operation-local SQL and mapper state
* Non-unit-of-work operations use an independent database connection scope per operation
* Unit-of-work operations retain the shared transaction connection for enlist, save, and rollback
* Regression tests (query and transaction) via XUnit
* Standard exception Logging
* Supports Paging
* Supports Filtering and Sorting
* Supports queries that return JSON
* Supports queries that return a scalar value
* Supports queries that don't return a value

### API 

#### Find All

```C#
Task<ICollection<TEntity>> FindAll()
```

Returns all active rows without criteria, sorting, or paging.

#### Find All with Pager

```C#
Task<IPager<TEntity>> FindAll(IPager<TEntity> pager)
```

Returns one page with sorting and paging. Repository instances may execute this concurrently with other operations.

#### Find All with Pager with Filter

```C#
Task<IPager<TEntity>> FindAllFiltered(IPager<TEntity> pager)
```

Returns one filtered and sorted page. The implementation supports the repository's allowlisted filter and sort columns.

#### Find By Primary Key

```C#
Task<TEntity> FindByPK(IPrimaryKey pk)
```

Returns one active row or `null` when no matching row exists.

#### Add

```C#
Task<object> Add(TEntity entity)
```

Adds a row and returns its identity key, or the affected-row count for natural/composite keys. When enlisted in a unit of work, the operation remains uncommitted until `Save()` succeeds.

#### Update

```C#
Task<int> Update(TEntity entity)
```

Updates an entity and returns the number of affected rows.

#### Delete

```C#
Task<int> Delete(PrimaryKey pk)
```

Soft-deletes an entity by setting `Active = 0` and returns the number of affected rows.

#### Unit of Work

```C#
Task<bool> Save()
Task<bool> Rollback()
```

`Save()` commits the managed `SqlTransaction` once. `Rollback()` rolls it back once. Subsequent terminal calls are harmless and do not begin a new transaction.

## Repository Concurrency

A repository instance can be shared by concurrent asynchronous operations. Each operation snapshots SQL text, command type, paging SQL, and entity mappers before its first database await, preventing concurrent calls from overwriting one another's command configuration. The shared `DBContext` is only used directly for unit-of-work transactions; ordinary repository calls create and dispose a separate connection scope.

Transaction use remains intentionally scoped: instantiate one `UnitOfWork` for the set of operations that must commit or roll back together. Commands enlisted in that unit of work are attached to one managed `SqlTransaction`. `Save` commits it once, `Rollback` rolls it back once, and repeated terminal calls are harmless. Do not overlap unrelated transaction operations on that unit of work.

## Updates

- Repository operations now use operation-local command state and disposable connection scopes
- Solution upgraded to Visual Studio 2022
- Projects target .NET 10
- Package dependencies updated, including Microsoft.Data.SqlClient

## FAQ
1. [Why doesn't the API support Sorting without paging?](#no-api-support-for-sorting-without-paging)

#### No API support for sorting without paging
If the result set is small enought, then just Find All and sort the result.  If it's long enough to need paging, then it's built-in.


## Related
  * [Repository Pattern](https://www.martinfowler.com/eaaCatalog/repository.html)