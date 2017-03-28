bcp Customer.dbo.State in backup\State.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2016
bcp Customer.dbo.City in backup\City.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2016
bcp Customer.dbo.Contact in backup\Contact.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2016
Pause