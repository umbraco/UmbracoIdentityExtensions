param (
	[Parameter(Mandatory=$true)]
	[ValidatePattern("^\d\.\d\.(?:\d\.\d$|\d$)")]
	[string]
	$ReleaseVersionNumber,
	[Parameter(Mandatory=$true)]
	[string]
	[AllowEmptyString()]
	$PreReleaseName
)

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path);
$RepoRoot = (get-item $PSScriptFilePath).Directory.Parent.FullName;
$SolutionRoot = Join-Path -Path $RepoRoot "src";

#trace
"Solution Root: $SolutionRoot"

$MSBuild = "$Env:SYSTEMROOT\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";

# Make sure we don't have a release folder for this version already
$BuildFolder = Join-Path -Path $RepoRoot -ChildPath "build";
$ReleaseFolder = Join-Path -Path $BuildFolder -ChildPath "Releases\v$ReleaseVersionNumber$PreReleaseName";
if ((Get-Item $ReleaseFolder -ErrorAction SilentlyContinue) -ne $null)
{
	Write-Warning "$ReleaseFolder already exists on your local machine. It will now be deleted."
	Remove-Item $ReleaseFolder -Recurse
}
New-Item $ReleaseFolder -Type directory

#trace
"Release path: $ReleaseFolder"

$AppStartFolder = Join-Path -Path $SolutionRoot -ChildPath "App_Start";
$AppStartDestFolder = Join-Path -Path $ReleaseFolder -ChildPath "App_Start";
Copy-Item "$AppStartFolder\*.*" -Destination (New-Item ($AppStartDestFolder) -Type directory);

# Rename all .cs files to .cs.pp
Get-ChildItem $AppStartDestFolder -Recurse -Filter *.cs | Rename-Item -newname {  $_.name  -Replace '\.cs$','.cs.pp'  }

# Replace the namespace with the token in each file
Get-ChildItem $AppStartDestFolder -Recurse -Filter *.pp |
Foreach-Object {
	(Get-Content $_.FullName) `
	-replace " Umbraco\.Web\.UI", " `$rootnamespace`$" |
	Set-Content $_.FullName -Encoding UTF8;
}

# COPY THE README OVER
Copy-Item "$BuildFolder\UmbracoCms.Identity.Readme.txt" -Destination $ReleaseFolder\Readme.txt

# Go get nuget.exe if we don't hae it
$NuGet = "$BuildFolder\nuget.exe"
$FileExists = Test-Path $NuGet 
If ($FileExists -eq $False) {
	$SourceNugetExe = "http://nuget.org/nuget.exe"
	Invoke-WebRequest $SourceNugetExe -OutFile $NuGet
}

# COPY OVER THE NUSPEC AND BUILD THE NUGET PACKAGE
Copy-Item "$BuildFolder\UmbracoCms.Identity.nuspec" -Destination $ReleaseFolder
$NuSpec = Join-Path -Path $ReleaseFolder -ChildPath "UmbracoCms.Identity.nuspec";
Write-Output "DEBUGGING: " $NuSpec -OutputDirectory $ReleaseFolder -Version $ReleaseVersionNumber$PreReleaseName
& $NuGet pack $NuSpec -OutputDirectory $ReleaseFolder -Version $ReleaseVersionNumber$PreReleaseName

""
"Build $ReleaseVersionNumber$PreReleaseName is done!"

