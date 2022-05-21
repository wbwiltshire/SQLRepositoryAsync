# SQLRepositoryAsync

A reference application written in C# .Net Core which implements the Repository pattern and uses ADO.NET to asynchronously access a SQL Server database.
Regression testing of queries and transactions is performed using XUnit.

## Requirements

### Features
* Implements the Repository Pattern
* All methods execute asynchronously
* Supports Unit of Work
* Regression tests (query and transaction) via XUnit
* Standard exception Logging
* Supports Paging
* Supports Filtering and Sorting
* Supports queries that return JSON
* Supports queries that return a scalar value
* Supports queries that don't return a value

### API 

#### Find All Count

```C#
	int FindAllCount()
```
Used to return total row count for query.  Used by FindAll with Pager.

#### Find All

```C#
	ICollection<TEntity> FindAll()
```
Return all rows without  criteria, sorting, or paging.


#### Find All with Pager
Return a single page with sorting.

```C#
	IPager<TEntity> FindAll(IPager<TEntity> pager)
```


#### Find All with Pager with Filter
Return a single page with filter and sorting.

```C#
	IPager<TEntity> FindAllFiltered(IPager<TEntity> pager)
```


#### Find By Primary Key
Return a single row using primary key.

```C#
	TEntity FindByPK(PrimaryKey pk)
```


#### Add
Add row and return the identity value of the row.

```C#
	object Add(TEntity entity, PrimaryKey pk)
```

#### Update
Update row based on Primary Key and return the number of rows affected.

```C#
	int Delete(TEntity entity, PrimaryKey pk)
```

#### Delete
Soft delete row based on Primary Key return the number of rows affected.

```C#
	int Update(PrimaryKey pk)
```

#### Execute Non-Query with parameters
Execute a SQL query which does not return a row(s) or only returns an integer value.

```C#
	int ExecNonQuery(IList<SQLParameter> parms)
```

#### ExecuteStoredProc
Execute a SQL Stored Procedure which does not return a row(s) or only returns an integer value.

```C#
	int ExecStoredProc(IList<SQLParameter> parms)
```

#### Execute Query and return JSON
Execute a query which returns a JSON string.

```C#
	string ExecJSONQuery(IList<SQLParameter> parms)
```

## Updates

- Solution was upgraded to VS2019 Community Edition
- .Net 5.0

## FAQ
1. [Why doesn't the API support Sorting without paging?](#no-api-support-for-sorting-without-paging)

#### No API support for sorting without paging
If the result set is small enought, then just Find All and sort the result.  If it's long enough to need paging, then it's built-in.


## Related
  * [Repository Pattern](https://www.martinfowler.com/eaaCatalog/repository.html)