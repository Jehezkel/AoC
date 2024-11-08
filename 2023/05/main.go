package main

import (
	_ "embed"
	"flag"
	"fmt"
	"regexp"
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

type inputMap struct {
	mapName string
	rows    []inputDataRow
}
type inputDataRow struct {
	dstStart int
	srcStart int
	rangeLen int
}

func main() {
	fmt.Println("Welcome to your template")

	var part int
	flag.IntVar(&part, "part", 1, "part 1 or 2")
	flag.Parse()

	var answer int
	inputArr := strings.Split(input, "\n")
	if part == 1 {
		answer = partOne(inputArr)
	} else {
		answer = partTwo(inputArr)
	}

	fmt.Println("Output:", answer)
}

func parseInput(input []string) ([]int, []inputMap) {
	numbersRegex := regexp.MustCompile(`[0-9]+`)
	// seeds parse
	seedMatches := numbersRegex.FindAllString(input[0], -1)
	seeds := *new([]int)
	for _, seedMatch := range seedMatches {
		if seedVal, err := strconv.Atoi(seedMatch); err == nil {
			seeds = append(seeds, seedVal)
		} else {
			panic(err)
		}

	}
	fmt.Println("Seeds", seeds)
	inputMaps := []inputMap{}
	var currMap *inputMap
	for i := 1; i < len(input); i++ {
		line := input[i]
		if len(line) == 0 {
			continue
		}

		lineFields := strings.Fields(line)

		if lineFields[1] == "map:" {
			fmt.Println("Adding new map:", lineFields[0])
			newMap := inputMap{
				mapName: lineFields[0]}
			inputMaps = append(inputMaps, newMap)
			currMap = &inputMaps[len(inputMaps)-1]
			continue
		}
		newRow := inputDataRow{
			dstStart: castToInt(lineFields[0]),
			srcStart: castToInt(lineFields[1]),
			rangeLen: castToInt(lineFields[2]),
		}
		currMap.rows = append(currMap.rows, newRow)
		fmt.Println(newRow)
		fmt.Println(currMap.rows)
	}
	fmt.Println("MAPS:", inputMaps)
	return seeds, inputMaps
}

type seedsRange struct {
	start int
	len   int
	end   int
}

func parseInputPtTwo(input []string) ([]seedsRange, []inputMap) {
	numbersRegex := regexp.MustCompile(`[0-9]+`)
	// seeds parse
	seedMatches := numbersRegex.FindAllString(input[0], -1)
	// seeds := *new([]int)
	seeds := []seedsRange{}
	var curSeedRange seedsRange
	for index, seedMatch := range seedMatches {
		if index%2 == 0 {
			curSeedRange = seedsRange{start: castToInt(seedMatch)}
			continue
		}
		curSeedRange.len = castToInt(seedMatch)
		curSeedRange.end = curSeedRange.start + curSeedRange.len
		seeds = append(seeds, curSeedRange)
	}
	// for _, seedMatch := range seedMatches {
	// 	if seedVal, err := strconv.Atoi(seedMatch); err == nil {
	// 		seeds = append(seeds, seedVal)
	// 	} else {
	// 		panic(err)
	// 	}
	//
	// }
	fmt.Println("Seeds", seeds)
	inputMaps := []inputMap{}
	var currMap *inputMap
	for i := 1; i < len(input); i++ {
		line := input[i]
		if len(line) == 0 {
			continue
		}

		lineFields := strings.Fields(line)

		if lineFields[1] == "map:" {
			fmt.Println("Adding new map:", lineFields[0])
			newMap := inputMap{
				mapName: lineFields[0]}
			inputMaps = append(inputMaps, newMap)
			currMap = &inputMaps[len(inputMaps)-1]
			continue
		}
		newRow := inputDataRow{
			dstStart: castToInt(lineFields[0]),
			srcStart: castToInt(lineFields[1]),
			rangeLen: castToInt(lineFields[2]),
		}
		currMap.rows = append(currMap.rows, newRow)
		fmt.Println(newRow)
		fmt.Println(currMap.rows)
	}
	fmt.Println("MAPS:", inputMaps)
	return seeds, inputMaps
}
func castToInt(str string) int {
	result, err := strconv.Atoi(str)
	if err != nil {
		panic(err)
	}
	return result
}

func partOne(input []string) int {
	seeds, maps := parseInput(input)
	seedsMap := make(map[int]int)
	minSeed := seeds[0]
	for _, seed := range seeds {
		currValue := seed
		for _, currMap := range maps {
			// fmt.Println("Map", currMap.mapName, "starting value", currValue)
			// fmt.Println(currMap.rows)
			for _, mapRow := range currMap.rows {
				if currValue < mapRow.srcStart || currValue >= mapRow.srcStart+mapRow.rangeLen {
					continue
				}
				currValue = currValue + (mapRow.dstStart - mapRow.srcStart)
				break
			}
		}
		seedsMap[seed] = currValue
		if currValue < seedsMap[minSeed] {
			minSeed = seed
		}
	}
	return seedsMap[minSeed]
}

func partTwo(input []string) int {
	seedRanges, maps := parseInputPtTwo(input)
	// minSeed := seedRanges[0]
	answer := 1<<63-1
	for _, seedRange := range seedRanges {
		for i := seedRange.start; i <= seedRange.end; i++ {
			seed := i
			currValue := seed
			for _, currMap := range maps {
				// fmt.Println("Map", currMap.mapName, "starting value", currValue)
				// fmt.Println(currMap.rows)
				for _, mapRow := range currMap.rows {
					if currValue < mapRow.srcStart || currValue >= mapRow.srcStart+mapRow.rangeLen {
						continue
					}
					currValue = currValue + (mapRow.dstStart - mapRow.srcStart)
					break
				}
			}
			if currValue < answer {
				answer= currValue
			}
		}
	}
	return answer
}
