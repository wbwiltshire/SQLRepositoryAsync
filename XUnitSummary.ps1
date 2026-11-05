#############################################################################################################
# .SYNOPSIS
#  Summarize the results of XUnit test run
#
# .DESCRIPTION
#  Read the TestResult.xml file (default) output by the XUnit Test runner and display
#  the results to the screen
#
# .PARAMETER InputFile
#  Input file name (alias -i)
#
# .PARAMETER v
#  A switch which when true prints out the script's version information (include type information)
#
# .PARAMETER NoWait
#  A switch which when true skips the final "Press <enter> to exit." prompt (for non-interactive runs)
#
# .PARAMETER SummaryOnly
#  A switch which when true shows only the totals (plus the names of any failed or skipped tests) instead of
#  a row per test. Every result is still in the input file.
#
# .EXAMPLE
#  XUnitSummary.ps1 -i <InputFile.xml> -v
#
#  Summarize the results of XUnit test run
#
#  .EXAMPLE
#  XUnitSummary.ps1 -v
#
#  Short-hand syntax of the script using default TestResult.xml file
#
# .NOTES
#  You don't need to run this script with any special permissions
#
#  Author: Warren Wiltshire (Warren@SeagullConsulting.Biz)
#  Date: 08/30/2020
#  Source: https://github.com/wbwiltshire/SQLite3Repository (XUnitSummary.ps1)
#  Change Log:
#     0.9.0(08/30/2020) - Initial script
#     0.9.1(10/28/2026) - EV Charge Tracker: -NoWait switch; Test column sized to the longest test name
#     0.9.2(10/28/2026) - EV Charge Tracker: -SummaryOnly switch
#     0.9.3(11/04/2026) - MeanGreen: test names ignore dots inside theory arguments, e.g. Method(value: 2.5)
#############################################################################################################
Param (
	[parameter(Mandatory=$false)]
	[alias ('i')] [string] $InputFile = "TestResults/TestResult.xml",
	[switch] $v = $false,
	[switch] $NoWait = $false,
	[switch] $SummaryOnly = $false
)
# Define functions
function ScriptVersion
{
  return '0.9.3';
}
# Method name (with any theory arguments) without the namespace and class, ignoring dots inside the arguments
function Get-TestDisplayName([string]$testName)
{
  $paren = $testName.IndexOf('(')
  $prefix = if ($paren -ge 0) { $testName.Substring(0, $paren) } else { $testName }
  return $testName.Substring($prefix.LastIndexOf('.') + 1)
}
# Main Process
$ElapsedTime = [System.Diagnostics.Stopwatch]::StartNew()
Write-Host "Script Started at $(get-date)"
$total = 0
$passed = 0
$failed = 0
$skipped = 0
$global:ECnt = 0
$psVersion = "{0}.{1}" -f $PSVersionTable.PSVersion.Major, $PSVersionTable.PSVersion.Minor
if ([System.Environment]::Is64BitProcess)
	{ Write-Host "Running PowerShell version $($psVersion) x64" }
else
	{ Write-Host "Running PowerShell version $($psVersion) x86" }

Try {
	$ver = ScriptVersion;

	if ($v)
		{ Write-Host "version: $ver"; }
	else {
		$output = "XUnit Test Summary({0}):" -f $InputFile
		Write-Host $output
		Write-Host ""
		[xml]$xml = Get-Content $InputFile
		$tests = $xml.TestRun.Results.UnitTestResult | Sort-Object -property testName

		# Size the Test column to the longest test (method) name
		$names = $tests | ForEach-Object { Get-TestDisplayName $_.testName }
		$width = [Math]::Max(19, ($names | Measure-Object -Property Length -Maximum).Maximum)
		if (-not $SummaryOnly) {
			Write-Host ("`t{0} `t Status `tDuration(secs)" -f "Test".PadRight($width))
			Write-Host ("`t{0}`t--------`t--------------" -f ("-" * $width))
		}

		$notPassed = @()
		foreach ($test in $tests) {
			$total++
			$output = Get-TestDisplayName $test.testName
			switch ($test.outcome.ToUpper()) {
				"PASSED" { $passed++ }
				"FAILED" { $failed++; $notPassed += [pscustomobject]@{ Name = $test.testName; Status = "Failed"; Color = "Red" } }
				"NOTEXECUTED" { $skipped++; $notPassed += [pscustomobject]@{ Name = $test.testName; Status = "Skipped"; Color = "Yellow" } }
			}
			if ($SummaryOnly) { continue }

			Write-Host ("`t{0}" -f $output.PadRight($width)) -NoNewline
			$output = "`t{0}" -f $test.outcome
			switch ($test.outcome.ToUpper()) {
				"PASSED" { Write-Host "$output" -f Green -NoNewline }
				"FAILED" { Write-Host "$output" -f Red -NoNewline }
				"NOTEXECUTED" { Write-Host "`tSkipped" -f Yellow -NoNewline }
			}
			$duration = ([TimeSpan]$test.duration).TotalSeconds
			$output = "{0:0.000}" -f $duration
			Write-Host "`t`t$output"
		}

		Write-Host ""
		$output = "Total: {0}, Passed: {1}, Failed: {2}, Skipped: {3}" -f $total, $passed, $failed, $skipped
		if ($SummaryOnly) {
			$color = if ($failed -gt 0) { "Red" } elseif ($skipped -gt 0) { "Yellow" } else { "Green" }
			Write-Host $output -f $color
			# Passing tests are only in the results file; name anything that needs attention.
			foreach ($t in $notPassed) { Write-Host ("`t{0,-8} {1}" -f $t.Status, $t.Name) -f $t.Color }
		}
		else
			{ Write-Host $output }
	}
}
Catch {
	$ex = $_.Exception
	$errLine = $_.InvocationInfo.ScriptLineNumber
	Write-Output ("`nError: {0} at line {1}`n" -f $ex.Message, $errLine)
	Exit 1
}

Write-Host ""
Write-Host "Done"
Write-Host "$ECnt Exceptions"
Write-Host "Script Ended at $(get-date)"
Write-Host "Total Elapsed Time: $($ElapsedTime.Elapsed.ToString())"
if (-not $NoWait)
	{ Read-Host "Press <enter> to exit." }
