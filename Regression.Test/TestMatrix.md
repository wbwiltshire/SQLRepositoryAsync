SQLRepositoryAsync Test Matrix
===


Test| SQL | SPROC | View
 -- | --- | ----- | ----
**Contact** | | |
FindAll          | P | NA | NA
SqlFailureIsPropagated | P | NA | NA
ProjectMappingFailureIsPropagated | P | NA | NA
ConcurrentSharedRepositoryOperations | P | NA | NA
FindAllPaged     | P | P  | NA
FindAllView      | P | NA | P
FindAllViewPaged | P | P  | P
FindByPK         | P | NA | NA
FindViewByPK     | P | P  | NA
Add              | P | P  | NA
Update           | P | P  | NA
Delete           | P | NA | NA
---------------- | --- | ----- | ----
**City** | | | 
FindAll          | P | NA | NA
FindAllPaged     | P | P  | NA
FindAllPagedDirectSql | P | NA | NA
FindAllView      | P | NA | P
FindAllViewPaged | P | P  | P
FindAllViewPagedStoredSort | P | P | P
FindByPK         | P | NA | NA
FindViewByPK     | P | NA | NA
Add              | P | P  | NA
Update           | P | P  | NA
Delete           | P | NA | NA
---------------- | --- | ----- | ----
**State** | | |
FindAll          | P  | NA | NA
FindAllPaged     | P  | NA | NA
FindAllPagedSorted | P | NA | NA
FindAllView      | NA | NA | NA
FindAllViewPaged | NA | NA | NA
FindByPK         | P  | NA | NA
FindViewByPK     | NA | NA | NA
Add              | P  | P  | NA
Update           | P  | P  | NA
Delete           | P  | NA | NA
---------------- | --- | ----- | ----
**Project** | | |
FindAllPagedDirectSql | P | NA | NA
UnsupportedViewsReturnExplicitFailure | P | NA | NA
UnsupportedViewsReturnExplicitFailure | P | NA | NA
FindByPK         | P  | NA  | NA
Add              | P  | NA  | NA
Delete           | P  | NA  | NA
---------------- | --- | ----- | ----
**Transactional** | | |
UOW                    | P   | NA  | NA
Scope dispose on close failure | P | NA | NA
Save                   | P   | NA  | NA
Rollback               | P   | NA  | NA
Terminal lifecycle     | P   | NA  | NA
Failed save cleanup    | P   | NA  | NA

