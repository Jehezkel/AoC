package main

import (
	_ "embed"
	"flag"
	"fmt"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string

func init() {
	input = strings.TrimRight(input, "\n")
	if len(input) == 0 {
		panic("empty input.txt")
	}
}
func main() {
	fmt.Println("Welcome to your template")

	var part int
	flag.IntVar(&part, "part", 1, "part 1 or 2")
	flag.Parse()

	var answer int
	inputArray := strings.Split(input, "\n")
	if part == 1 {
		answer = partOne(inputArray)
	} else {
		answer = partTwo(inputArray)
	}

	fmt.Println("Output:", answer)
}

func partOne(input []string) int {
	result := 0
	for _, cardLine := range input {
		cardScore := 0
		carLineSplit := strings.Split(strings.Split(cardLine, ":")[1], "|")
		winningNumbers := strings.Fields(carLineSplit[0])
		myNumbers := strings.Fields(carLineSplit[1])
		fmt.Println("Winning:", winningNumbers)
		winningHashset := make(map[int]bool)
		for _, winningNumber := range winningNumbers {
			numValue, err := strconv.Atoi(strings.TrimSpace(winningNumber))
			if err != nil {
				panic(err)
			}
			winningHashset[numValue] = true
		}
		fmt.Println("My numbers:", myNumbers)
		for _, myNumber := range myNumbers {
			fmt.Println(myNumber)
			numValue, err := strconv.Atoi(strings.TrimSpace(myNumber))
			if err != nil {
				panic(err)
			}
			if winningHashset[numValue] {
				if cardScore == 0 {
					cardScore = 1
				} else {
					cardScore = cardScore << 1
				}
			}
		}
		result += cardScore
	}
	return result
}

type gameCardInfo struct {
	score  int
	copies int
}

func partTwo(input []string) int {
	gamesMap := make(map[int]gameCardInfo)
	for _, cardLine := range input {
		cardScore := 0
		cardId, err := strconv.Atoi(strings.Fields(strings.Split(cardLine, ":")[0])[1])

		if err != nil {
			panic(err)
		}
		carLineSplit := strings.Split(strings.Split(cardLine, ":")[1], "|")
		winningNumbers := strings.Fields(carLineSplit[0])
		myNumbers := strings.Fields(carLineSplit[1])
		fmt.Println("Winning:", winningNumbers)
		winningHashset := make(map[int]bool)
		for _, winningNumber := range winningNumbers {
			numValue, err := strconv.Atoi(strings.TrimSpace(winningNumber))
			if err != nil {
				panic(err)
			}
			winningHashset[numValue] = true
		}
		fmt.Println("My numbers:", myNumbers)
		for _, myNumber := range myNumbers {
			numValue, err := strconv.Atoi(strings.TrimSpace(myNumber))
			if err != nil {
				panic(err)
			}
			if winningHashset[numValue] {
				cardScore++
			}
		}
		gamesMap[cardId] = gameCardInfo{cardScore, 1}
	}
	result := 0
	for gameId := 1; gameId <= len(gamesMap); gameId++ {
		gameCardInfo:= gamesMap[gameId]
		fmt.Println("gameId:", gameId, "gameCardInfo", gameCardInfo)
		result += gameCardInfo.copies
		for i := gameId + 1; i <= gameId+gameCardInfo.score; i++ {
			changeGameInfo := gamesMap[i]
			changeGameInfo.copies += gameCardInfo.copies
			gamesMap[i] = changeGameInfo
		}
	}
	//test
	for gameId, gameInfo := range gamesMap {

		fmt.Println(gameId, gameInfo)
	}
	return result
}
