#############################################################################################################
# .SYNOPSIS
#  Create an object to hold Customer Database credentials
#
# .DESCRIPTION
#  Create a Powershell object for Customer Database credentials (username and password) and return it
#
# .PARAMETER Username
#  Database user name
#
# .PARAMETER Password
#  User password
#
# .PARAMETER v
#  A switch which when true prints out the script's version information (include type information)
#  
# .EXAMPLE
#  $solarDB = ./Set-SolarDB.ps1 -Username <username> -Password '<pw>'
#
# .EXAMPLE
#  $solarDB = ./Set-SolarDB.ps1 -u <username> -p '<pw>'-v
#
# .NOTES
#  You don't need to run this script with any special permissions
#
#  Your password should be enclosed in single quotes (e.g. '')
# 
#   Author: Warren Wiltshire (Warren@SeagullConsulting.Biz)
#   Date: 03/30/2024
#   Change Log:
#     0.9.0(03/30/2024) - Initial script
#############################################################################################################
Param (
	[parameter(Mandatory=$true)]
	[alias ('u')] [string] $Username,
	[parameter(Mandatory=$true)]
	[alias ('p')] [string] $Password,
	[switch] $v = $false	
)
# Define functions
function ScriptVersion
{
  return '0.9.0';
}
#Main Process
$ElapsedTime = [System.Diagnostics.Stopwatch]::StartNew()
Write-Host "Script Started at $(get-date)"
$global:ECnt = 0
$solarDB = $null

Try {	
        $ver = ScriptVersion
		if ($v)
			{ Write-Host "version: $ver"; }
		else {
			$solarDB = [PSCustomObject]@{ "Username"= $Username; "Password"= $Password; "CreatedOnUTC" = Get-Date -AsUTC  }
		}
}
Catch [System.Exception]
{
    $errLine = $_.InvocationInfo.ScriptLineNumber
	Write-Output ("Error: {0} at line {1}" -f $ex.Message, $errLine)
	$ECnt += 1
}

Write-Host "Done"
Write-Host "$ECnt Exception Errors"
Write-Host "Script Ended at $(get-date)"
Write-Host "Total Elapsed Time: $($ElapsedTime.Elapsed.ToString())"
return $solarDB