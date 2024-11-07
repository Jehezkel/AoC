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
	inputArray := strings.Split(input, "\n")
	var answer int
	if part == 1 {
		answer = partOne(inputArray)
	} else {
		answer = partTwo(inputArray)
	}

	fmt.Println("Output:", answer)
}

func partOne(input []string) int {
	maxRed := 12
	maxGreen := 13
	maxBlue := 14
	result := 0

	for _, line := range input {
		maxRoundRed, maxRoundGreen, maxRoundBlue := 0, 0, 0
		fmt.Println(line)
		lineSplit := strings.Split(line, ":")
		gameId, err := strconv.Atoi(strings.Split(lineSplit[0], " ")[1])

		if err != nil {
			panic(err)
		}
		cubeSets := strings.Split(lineSplit[1], ";")
		for _, cubeSetLine := range cubeSets {

			setBlue, setGreen, setRed := 0, 0, 0
			cubeInfos := strings.Split(cubeSetLine, ",")
			for _, cubeInfoString := range cubeInfos {
				cubeInfo := strings.Split(strings.TrimSpace(cubeInfoString), " ")
				cubeColor, cubeCountString := cubeInfo[1], cubeInfo[0]
				cubeCount, err := strconv.Atoi(cubeCountString)

				if err != nil {
					panic(err)

				}

				if cubeColor == "blue" {
					setBlue += cubeCount
				} else if cubeColor == "green" {
					setGreen += cubeCount
				} else {
					setRed += cubeCount
				}

			}
			if setBlue > maxRoundBlue {
				maxRoundBlue = setBlue
			}
			if setGreen > maxRoundGreen {
				maxRoundGreen = setGreen
			}
			if setRed > maxRoundRed {
				maxRoundRed = setRed
			}
		}
		if maxRoundBlue <= maxBlue && maxRoundGreen <= maxGreen && maxRoundRed <= maxRed {
			fmt.Println(gameId)
			result += gameId
		}
	}
	return result
}

func partTwo(input []string) int {

	result := 0

	for _, line := range input {
		maxRoundRed, maxRoundGreen, maxRoundBlue := 0, 0, 0
		fmt.Println(line)
		lineSplit := strings.Split(line, ":")
		// gameId, err := strconv.Atoi(strings.Split(lineSplit[0], " ")[1])

		// if err != nil {
		// 	panic(err)
		// }
		cubeSets := strings.Split(lineSplit[1], ";")
		for _, cubeSetLine := range cubeSets {

			setBlue, setGreen, setRed := 0, 0, 0
			cubeInfos := strings.Split(cubeSetLine, ",")
			for _, cubeInfoString := range cubeInfos {
				cubeInfo := strings.Split(strings.TrimSpace(cubeInfoString), " ")
				cubeColor, cubeCountString := cubeInfo[1], cubeInfo[0]
				cubeCount, err := strconv.Atoi(cubeCountString)

				if err != nil {
					panic(err)

				}

				if cubeColor == "blue" {
					setBlue += cubeCount
				} else if cubeColor == "green" {
					setGreen += cubeCount
				} else {
					setRed += cubeCount
				}

			}
			if setBlue > maxRoundBlue {
				maxRoundBlue = setBlue
			}
			if setGreen > maxRoundGreen {
				maxRoundGreen = setGreen
			}
			if setRed > maxRoundRed {
				maxRoundRed = setRed
			}
		}
		roundPower := maxRoundBlue * maxRoundRed * maxRoundGreen
		result += roundPower
	}
	return result
}
