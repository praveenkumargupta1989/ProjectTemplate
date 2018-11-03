$path = "C:\WorkArea\ACIPL\Template\Code\Catlyst\"
$templateName = "ACIPL.Template"
$newName = "ACIPL.Catalyst"

$csPath = $path+"*.cs"

gci $csPath -recurse | ForEach {
  (Get-Content $_ | ForEach {$_ -replace $templateName, $newName }) | Set-Content $_ 
}

$csProjPath = $path+"*.csproj"

gci $csProjPath -recurse | ForEach {
  (Get-Content $_ | ForEach {$_ -replace $templateName, $newName }) | Set-Content $_ 
}

$slnPath = $path+"*.sln"

gci $slnPath -recurse | ForEach {
  (Get-Content $_ | ForEach {$_ -replace $templateName, $newName }) | Set-Content $_ 
}

$asxPath = $path+"*.asax"

gci $asxPath -recurse | ForEach {
  (Get-Content $_ | ForEach {$_ -replace $templateName, $newName }) | Set-Content $_ 
}

Get-ChildItem $path -Recurse -Directory | ForEach-Object {

$dirPath = $_.FullName
Write-Host $dirPath+"\"
get-childItem $dirPath -recurse -include '*ACIPL.Template*.*' | rename-item -newname { $_.name -replace 'ACIPL.Template', 'ACIPL.Catalyst' }
}

get-childItem $path -recurse -include '*.*' | rename-item -newname { $_.name -replace 'ACIPL.Template', 'ACIPL.Catalyst' }
