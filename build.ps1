<#
.SYNOPSIS 
    You can use this script to exeucte the build for Markdown web. The logic for the build
    process is captured in 'build.proj.
#>
[cmdletbinding()]
param(
    $Configuration = 'Release',
    $targets = @(),
    $toolsDir = ("$env:LOCALAPPDATA\LigerShark\tools\"),
    $nugetDownloadUrl = 'http://nuget.org/nuget.exe'
)

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$scriptDir = ((Get-ScriptDirectory) + "\")

<#
.SYNOPSIS 
    This will throw an error if the psbuild module is not installed and available.
#>
function EnsurePsbuildInstalled(){
    [cmdletbinding()]
    param()
    process{

        if(!(Get-Module -listAvailable 'psbuild')){
            $msg = ('psbuild is required for this script, but it does not look to be installed. Get psbuild from here: https://github.com/ligershark/psbuild')
            throw $msg
        }

        if(!(Get-Module 'psbuild')){
            # add psbuild to the currently loaded session modules
            import-module psbuild -Global;
        }
    }
}

<#
.SYNOPSIS
    If nuget is not in the tools
    folder then it will be downloaded there.
#>
function Get-Nuget(){
    [cmdletbinding()]
    param(
        $toolsDir = ("$env:LOCALAPPDATA\LigerShark\AzureJobs\tools\"),

        $nugetDownloadUrl = 'http://nuget.org/nuget.exe'
    )
    process{
        $nugetDestPath = Join-Path -Path $toolsDir -ChildPath nuget.exe

        if(!(Test-Path $toolsDir)){
            New-Item -Path $toolsDir -ItemType Directory | Out-Null
        }
        
        if(!(Test-Path $nugetDestPath)){
            'Downloading nuget.exe' | Write-Verbose
            # download nuget
            $webclient = New-Object System.Net.WebClient
            $webclient.DownloadFile($nugetDownloadUrl, $nugetDestPath)

            # double check that is was written to disk
            if(!(Test-Path $nugetDestPath)){
                throw 'unable to download nuget'
            }
        }

        # return the path of the file
        $nugetDestPath
    }
}

###########################################################
# Begin script
###########################################################

'
Build started
This script uses psbuild which is available at http://aka.ms/psbuild
' | Write-Host

EnsurePsbuildInstalled
'Restoring nuget packages' | Write-Host
$slnFile = Get-Item (Join-Path $scriptDir 'MarkdownLog.sln')
# restore nuget packages
$nugetArgs = @('restore',$slnFile.FullName)

&(Get-Nuget -toolsDir $toolsDir -nugetDownloadUrl $nugetDownloadUrl) $nugetArgs

$buildFile = (Get-Item (Join-Path $scriptDir 'build.proj')).FullName

Invoke-MSBuild -projectsToBuild $buildFile `
                -configuration $Configuration `
                -targets $targets