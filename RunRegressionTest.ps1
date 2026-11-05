#Requires -Version 7
<#
.SYNOPSIS
    Builds and runs the SQLRepositoryAsync regression tests, writes the results to TestResults\TestResult.xml (TRX),
    and displays a summary.
.DESCRIPTION
    Same layout as MeanGreen's RunRegressionTest.ps1 and EVChargeTracker's Run-Tests.ps1: xUnit v3 on
    Microsoft.Testing.Platform (see global.json). The results file is summarized with XUnitSummary.ps1.

    Database
    --------
    The tests run against the live SQL Server database configured in Regression.Test\appsettings.json. The
    ${DB_PASSWORD} placeholder in its connection string is replaced by the DB_PASSWORD environment variable.

    Set it for the current PowerShell session (single quotes, so $ in the password isn't expanded):
      $env:DB_PASSWORD = '<password>'

    Or for your Windows user (persists across sessions):
      [Environment]::SetEnvironmentVariable('DB_PASSWORD', '<password>', 'User')

    The transaction tests change the seed data. Reload the database from the SQL directory before each run so it
    starts in a clean state (ReloadDatabase.ps1 reads the credentials from $DBCustomer):
      cd SQL
      $DBCustomer = ./Set-DBCustomer.ps1 -Username <username> -Password '<password>'
      ./ReloadDatabase.ps1
.PARAMETER FilterClass
    Run only this test class, e.g. Regression.Test.QueryTests
.PARAMETER FilterMethod
    Run only this test method, e.g. Regression.Test.TransactionTests.SaveTest
.PARAMETER NoBuild
    Skip the build and run the existing test binaries.
.PARAMETER NoSummary
    Don't run XUnitSummary.ps1 afterwards.
.PARAMETER NoWait
    Don't wait for <enter> at the end of the summary.
.PARAMETER SummaryOnly
    Show only the totals (plus any failed or skipped test names) instead of a row per test.
    Every result is still written to TestResults\TestResult.xml.
.EXAMPLE
    .\RunRegressionTest.ps1
.EXAMPLE
    .\RunRegressionTest.ps1 -SummaryOnly
.EXAMPLE
    .\RunRegressionTest.ps1 -FilterClass Regression.Test.QueryTests -NoWait
#>
param(
    [string]$FilterClass,
    [string]$FilterMethod,
    [switch]$NoBuild,
    [switch]$NoSummary,
    [switch]$NoWait,
    [switch]$SummaryOnly
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$project = Join-Path $root 'Regression.Test/Regression.Test.csproj'
$resultsDir = Join-Path $root 'TestResults'
$resultFile = Join-Path $resultsDir 'TestResult.xml'

# Pick up a user variable created after this PowerShell window was opened
if ([string]::IsNullOrWhiteSpace($env:DB_PASSWORD)) {
    $env:DB_PASSWORD = [Environment]::GetEnvironmentVariable('DB_PASSWORD', 'User')
}
if ([string]::IsNullOrWhiteSpace($env:DB_PASSWORD)) {
    Write-Host 'DB_PASSWORD is not set - the database tests will fail to connect. Set $env:DB_PASSWORD first.' -ForegroundColor Yellow
}
Write-Host 'Reminder: run SQL\ReloadDatabase.ps1 first so the transaction tests start from clean seed data.' -ForegroundColor Cyan

Push-Location $root
try {
    # Never summarize a stale results file from an earlier run.
    if (Test-Path $resultFile) { Remove-Item $resultFile }

    if (-not $NoBuild) {
        dotnet build $project
        if ($LASTEXITCODE -ne 0) { throw "Build failed (exit code $LASTEXITCODE)." }
    }

    $testArgs = @('test', '--project', $project, '--no-build',
        '--report-xunit-trx', '--report-xunit-trx-filename', 'TestResult.xml', '--results-directory', $resultsDir)
    if ($FilterClass) { $testArgs += @('--filter-class', $FilterClass) }
    if ($FilterMethod) { $testArgs += @('--filter-method', $FilterMethod) }

    dotnet @testArgs
    $testExitCode = $LASTEXITCODE

    if (-not $NoSummary) {
        if (Test-Path $resultFile) { & (Join-Path $root 'XUnitSummary.ps1') -InputFile $resultFile -NoWait:$NoWait -SummaryOnly:$SummaryOnly }
        else { Write-Warning "No results file was written to $resultFile." }
    }
}
finally {
    Pop-Location
}

exit $testExitCode
