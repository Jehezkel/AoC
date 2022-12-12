[CmdletBinding()]
param (
    [string]
    $FilePath="./input.txt"
)

$fullPath = Get-Item -Path $FilePath | Select -ExpandProperty FullName

$reader = New-Object -TypeName System.IO.StreamReader -ArgumentList $fullPath
$current_rank = [System.Collections.ArrayList]::new()
$current_sum = 0
$isEnd=$false
while (($line = $reader.ReadLine()) -ne $null) {
    $current_sum+=[int]$line
    Write-Host "line : $line : Sum: $current_sum" 
    Write-Host "rank: `n$current_rank" 
    if($line -eq '' -or $reader.EndOfStream){
        $current_rank.Add($current_sum)
        if($current_rank.Count -gt 3){
            [System.Collections.ArrayList]$current_rank =  $current_rank | sort -Descending | select -First 3
        }
        $current_sum=0
    }
}

$reader.Close()
$result = 0
$current_rank | % {$result+=$_}
Write-host $result