SQLCMD -i DropTables.sql    -S MSSQLSVR2022WEB.SeagullConsulting.Biz -U $DBCustomer.Username -P $DBCustomer.Password 
SQLCMD -i CreateState.sql   -S MSSQLSVR2022WEB.SeagullConsulting.Biz -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -i CreateCity.sql    -S MSSQLSVR2022WEB.SeagullConsulting.Biz -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -i CreateContact.sql -S MSSQLSVR2022WEB.SeagullConsulting.Biz -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -i CreateFKConstraints.sql -S MSSQLSVR2022WEB.SeagullConsulting.Biz -U $DBCustomer.Username -P $DBCustomer.Password
Pause