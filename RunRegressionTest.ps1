## Don't forget to set the password as below
## $env:DB_PASSWORD = "<Provide SuperSecretPassword>"

## Run ReloadDatabase from the SQL directory, before running this script to ensure the database is in a clean state.

dotnet build /p:Platform="Any CPU"
dotnet vstest .\Regression.Test\bin\Debug\net10.0\Regression.Test.dll --logger:"trx;LogFileName=TestResult.xml"

## Run XUnitSummary.ps1 after the test run to generate a summary of the test results.
pause