dotnet build
dotnet vstest .\Regression.Test\bin\Debug\netcoreapp1.1\Regression.Test.dll --logger:trx;LogFileName=TestResult.xml
pause