[CmdletBinding()]
param (
    [string]
    $filePath=".\INPUT.txt"
)
# A X - ROCK
# B Y - PAPER
# C Z - SCISSORS
enum GameChoice {
    Rock=1
    Paper=2
    Scissors=3
}
class Round {
    Round($roundLine) {
        Write-Host "Line: $roundLine"
        $lineAsArray = $roundLine.Split(' ')
        $lineAsArray | % {
            switch ($_) {
                'A' {$this.enemy = [GameChoice]::Rock;  break;}
                'B' {$this.enemy = [GameChoice]::Paper;  break;}
                'C' {$this.enemy = [GameChoice]::Scissors;  break;}
                Default {$this.me = $this.GetShapeForResult($_);  break;}
            } 
        }
        Write-Host "`tme: $($this.me)"
        Write-Host "`tenemy: $($this.enemy)"
    }
    [GameChoice]$me
    [GameChoice]$enemy
    [int]GetChoiceValue(){
        return $([int]$this.me)
    }
    [GameChoice]GetShapeForResult($choice){
        $result=[GameChoice]::Rock
        if($choice -eq 'Y'){
            Write-host "i need to draw"
            return $this.enemy
        }elseif ($choice -eq 'Z') {
            Write-host "i need to Win"

            switch ($this.enemy) {
                {$this.enemy -eq[GameChoice]::Scissors} { $result= [GameChoice]::Rock ;  break;}
                {$this.enemy-eq [GameChoice]::Rock} { $result= [GameChoice]::Paper ;  break;}
                {$this.enemy-eq [GameChoice]::Paper} { $result= [GameChoice]::Scissors ;  break;}
            }
        }elseif($choice -eq 'X'){
            Write-host "i need to loose"

            switch ($this.enemy) {
                {$this.enemy -eq[GameChoice]::Scissors} { $result= [GameChoice]::Paper ;  break;}
                {$this.enemy-eq [GameChoice]::Rock}{ $result= [GameChoice]::Scissors ;  break;}
                {$this.enemy-eq [GameChoice]::Paper} { $result= [GameChoice]::Rock ;  break;}
            }
        }else{
            Write-Error "BAD!"
        }
        return $result
    }
    [int]GetRoundResult(){
        $result = 0
        switch ($this) {
            {$($this.me -eq $this.enemy)} { Write-host "draw";$result=3;break;  }
            {$($($this.me -eq [GameChoice]::Paper) -and $($this.enemy -eq [GameChoice]::Rock) )} { Write-host "won";$result=6;break; }
            {$($($this.me -eq [GameChoice]::Scissors) -and $($this.enemy -eq [GameChoice]::Paper) )} { Write-host "won";$result=6;break;  }
            {$($($this.me -eq [GameChoice]::Rock) -and $($this.enemy -eq [GameChoice]::Scissors) )} { Write-host "won";$result=6;break; }
            Default {Write-host "lost";$result=0;break; }
        }
        return $result
    }
    [int]RoundScore(){
        $choiceValue = $this.GetChoiceValue()
        $result = $this.GetRoundResult()
        Write-Host "Choce val:$choiceValue `nResult: $result"
        return $choiceValue+$result
    }
}
$fullFilePath = Get-Item -Path $filePath | Select -ExpandProperty FullName
$reader = New-Object -TypeName System.IO.StreamReader -ArgumentList $fullFilePath
$totalScore = 0
$numOfRound = 0
while(($line = $reader.ReadLine())-ne $null){
    $roundObj = [Round]::new($line)
    $totalScore+= $roundObj.RoundScore()
    $numOfRound+=1
    Write-host "current score: $totalScore"
    write-host "---------------------------------"
}
Write-host $numOfRound
Write-host $totalScore