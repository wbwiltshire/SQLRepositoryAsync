SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i DropForeignKeyConstraints.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i DropTables.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateState.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateCity.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateContact.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateProject.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateProjectContact.sql -U dbuser -P P@ssw0rd
SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i CreateForeignKeyConstraints.sql -U dbuser -P P@ssw0rd
REM SQLCMD -S SCHVW2K12R2-DB\MSSQL2016 -i GrantStoredProcedureExecutePermissions.sql
bcp DCustomer.dbo.State in backup\State.txt -c -t "|"  -S SCHVW2K12R2-DB\MSSQL2016 -e bcp.log -U dbuser -P P@ssw0rd
bcp DCustomer.dbo.City in backup\City.txt -c -t "|" -S SCHVW2K12R2-DB\MSSQL2016 -e bcp.log -U dbuser -P P@ssw0rd
bcp DCustomer.dbo.Contact in backup\Contact.txt -c -t "|" -S SCHVW2K12R2-DB\MSSQL2016 -e bcp.log -U dbuser -P P@ssw0rd
bcp DCustomer.dbo.Project in backup\Project.txt -c -t "|" -S SCHVW2K12R2-DB\MSSQL2016 -e bcp.log -U dbuser -P P@ssw0rd
bcp DCustomer.dbo.ProjectContact in backup\ProjectContact.txt -c -t "|" -S SCHVW2K12R2-DB\MSSQL2016 -e bcp.log -U dbuser -P P@ssw0rd
Pause