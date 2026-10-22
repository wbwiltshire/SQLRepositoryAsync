SQLCMD -S MSSQLSVR2022WEB -i DropFKConstraints.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i DropTables.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateState.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateCity.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateContact.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateProject.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateProjectContact.sql -U $DBCustomer.Username -P $DBCustomer.Password
SQLCMD -S MSSQLSVR2022WEB -i CreateFKConstraints.sql -U $DBCustomer.Username -P $DBCustomer.Password
## SQLCMD -S MSSQLSVR2022WEB -i GrantStoredProcedureExecutePermissions.sql -U $DBCustomer.Username -P $DBCustomer.Password
bcp DCustomer.dbo.State in backup\State.txt -c -t "|"  -S MSSQLSVR2022WEB -e bcp.log -U $DBCustomer.Username -P $DBCustomer.Password 
bcp DCustomer.dbo.City in backup\City.txt -c -t "|" -S MSSQLSVR2022WEB -e bcp.log -U $DBCustomer.Username -P $DBCustomer.Password 
bcp DCustomer.dbo.Contact in backup\Contact.txt -c -t "|" -S MSSQLSVR2022WEB -e bcp.log -U $DBCustomer.Username -P $DBCustomer.Password 
bcp DCustomer.dbo.Project in backup\Project.txt -c -t "|" -S MSSQLSVR2022WEB -e bcp.log -U $DBCustomer.Username -P $DBCustomer.Password 
bcp DCustomer.dbo.ProjectContact in backup\ProjectContact.txt -c -t "|" -S MSSQLSVR2022WEB -e bcp.log -U $DBCustomer.Username -P $DBCustomer.Password 
Pause