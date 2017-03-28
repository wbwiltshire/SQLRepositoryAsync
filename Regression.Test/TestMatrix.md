SQLRepositoryAsync Test Matrix
===


Test| SQL | SPROC | View
 -- | --- | ----- | ----
**Contact** | | |
FindAll          | P | NA | NA
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
FindAllView      | P | NA | P
FindAllViewPaged | P | P  | P
FindByPK         | P | NA | NA
FindViewByPK     | P | NA | NA
Add              | P | P  | NA
Update           | P | P  | NA
Delete           | P | NA | NA
---------------- | --- | ----- | ----
**State** | | |
FindAll          | P  | NA | NA
FindAllPaged     | P  | NA | NA
FindAllView      | NA | NA | NA
FindAllViewPaged | NA | NA | NA
FindByPK         | P  | NA | NA
FindViewByPK     | NA | NA | NA
Add              | P  | P  | NA
Update           | P  | P  | NA
Delete           | P  | NA | NA
---------------- | --- | ----- | ----
**Transactional** | | |
UOW      | P   | NA  | NA
Save     | P   | NA  | NA
Rollback | P   | NA  | NA

