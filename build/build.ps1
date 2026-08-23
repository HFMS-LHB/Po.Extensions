# Po.Framework NuGet Pack Script

param(
    [string]$Version = ""
)

$ErrorActionPreference = "Stop"

# 当前脚本目录（解决方案根目录）
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path

$root = Split-Path -Parent $scriptPath

# 输出目录
$output = Join-Path $root "nupkg"

# 清理旧包
if (Test-Path $output) {
    Remove-Item $output -Recurse -Force
}

New-Item -ItemType Directory -Path $output | Out-Null


# 项目列表
$projects = @(
    "src/Po.MVVM.Core/Po.MVVM.Core.csproj",
    "src/Po.Navigation.Core/Po.Navigation.Core.csproj",
    "src/Po.Navigation.Avalonia/Po.Navigation.Avalonia.csproj",
    "src/Po.DialogHost.Avalonia/Po.DialogHost.Avalonia.csproj"
)


Write-Host "Restore solution..."

Push-Location $root

dotnet restore

Pop-Location


foreach ($project in $projects)
{
    $projectPath = Join-Path $root $project

    if (!(Test-Path $projectPath))
    {
        throw "Project not found: $projectPath"
    }


    Write-Host ""
    Write-Host "Packing $project ..." -ForegroundColor Green

    if ([string]::IsNullOrEmpty($Version))
    {
        dotnet pack `
            $projectPath `
            -c Release `
            --no-restore `
            -o $output
    }
    else
    {
        dotnet pack `
            $projectPath `
            -c Release `
            --no-restore `
            -p:Version=$Version `
            -o $output
    }

    if ($LASTEXITCODE -ne 0)
    {
        throw "Pack failed: $project"
    }
}


Write-Host ""
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "NuGet packages generated:" -ForegroundColor Cyan
Write-Host "Output: $output" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan


Get-ChildItem $output -Filter *.nupkg |
    Select-Object Name