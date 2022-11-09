dotnet build /p:Platform="Any CPU"
dotnet vstest .\Regression.Test\bin\Debug\net6.0\Regression.Test.dll --logger:trx;LogFileName=TestResult.xml
pause