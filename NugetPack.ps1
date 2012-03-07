[string[]] $buildFiles = '.\src\Utility.Autofac\Utility.Autofac.csproj', 
                         '.\src\Utility\Utility.AutofacSL5.csproj'  
[string[]] $nuspecFiles = '.\nuspec\Utility.Autofac.nuspec'
$versionFile = '.\src\SharedAssemblyInfo.cs'

$buildConfiguration = 'Release'
$outputPath = "$home\Dropbox\Packages"

Import-Module BuildUtilities

$version = Get-Version (Resolve-Path $versionFile)
  
New-Path $outputPath


#foreach($buildFile in $buildFiles)
#{
#  Invoke-Build (Resolve-Path $buildFile) $buildConfiguration
#}

foreach($nuspecFile in $nuspecFiles)
{
  New-Package (Resolve-Path $nuspecFile) $version $outputPath
}

Remove-Module BuildUtilities

