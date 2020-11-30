dotnet build /p:Platform="Any CPU"
dotnet vstest .\Regression.Test\bin\Debug\net5.0\Regression.Test.dll --logger:trx;LogFileName=TestResult.xml
pause