Param (
    [Parameter(HelpMessage="Deployment target resource group")] 
    [string] $ResourceGroupName = "rg-amazerrr-local",

    [Parameter(HelpMessage="Deployment target storage account name")] 
    [string] $WebStorageName,

    [Parameter(HelpMessage="App root folder path to publish e.g. ..\src\AmazerrrWeb\wwwroot\")] 
    [string] $AppRootFolder
)

$ErrorActionPreference = "Stop"

function GetContentType([string] $extension)
{
    if ($extension -eq ".html") 
    {
        return "text/html"
    }
    elseif ($extension -eq ".svg") 
    {
        return "image/svg+xml"
    }
    elseif ($extension -eq ".css") 
    {
        return "text/css"
    }
    elseif ($extension -eq ".js") 
    {
        return "text/javascript"
    }
    elseif ($extension -eq ".json") 
    {
        return "application/json"
    }
    return "text/plain"
}

Write-Host "Processing folder: $AppRootFolder"

if ($AppRootFolder.EndsWith("\") -eq $false)
{
    $AppRootFolder += "\"
}

$storageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -AccountName $WebStorageName
Get-ChildItem -File -Recurse $AppRootFolder `
    | ForEach-Object  { 
        $name = $_.FullName.Replace($AppRootFolder,"")
        $contentType = GetContentType($_.Extension)
        $properties = @{"ContentType" = $contentType}

        Write-Host "Deploying file: $name"
        Set-AzStorageBlobContent -File $_.FullName -Blob $name -Container `$web -Context $storageAccount.Context -Properties $properties -Force
    }

$webStorageUri = $storageAccount.PrimaryEndpoints.Web
Write-Host "Static website endpoint: $webStorageUri"
