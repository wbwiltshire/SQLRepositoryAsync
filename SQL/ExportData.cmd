bcp Customer.dbo.State out backup\State.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2016
bcp Customer.dbo.City out backup\City.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2016
bcp Customer.dbo.Contact out backup\Contact.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2016 
Pause