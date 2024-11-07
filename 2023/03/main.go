package main

import (
	_ "embed"
	"flag"
	"fmt"
	"math/big"
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
func main() {
	fmt.Println("Welcome to your template")

	var part int
	flag.IntVar(&part, "part", 1, "part 1 or 2")
	flag.Parse()

	var answer int
	inputSliced := strings.Split(input, "\n")
	if part == 1 {
		answer = partOne(inputSliced)
	} else {
		answer = partTwo(inputSliced)
	}

	fmt.Println("Output:", answer)
}


func partOne(input []string) int {
	numbersRegexp := regexp.MustCompile("[0-9]+")
	symbolsRegexp := regexp.MustCompile(`[^\w|.]`)

	var symbols []big.Int
	for index, line := range input {
		fmt.Println(index, line)
		symbolsResult := symbolsRegexp.FindAllStringIndex(line, -1)
		lineSymbols := big.NewInt(0)
		lenLine := len(line)
		for _, symbolMatch := range symbolsResult {
			fmt.Println(symbolMatch[1])
			symbolToAdd := new(big.Int).Lsh(big.NewInt(1), uint(lenLine)-uint(symbolMatch[1]))
			fmt.Printf("%010b\n", symbolToAdd)
			lineSymbols.Or(lineSymbols, symbolToAdd)
		}
		fmt.Printf("%010b\n", lineSymbols)
		symbols = append(symbols, *lineSymbols)
	}

	answer := 0
	for index, line := range input {
		results := numbersRegexp.FindAllStringIndex(line, -1)
		lineLen := len(line)
		for _, match := range results {

			numberString := line[match[0]:match[1]]
			fmt.Println("Line", line, "match:", match)
			wordMask := getRangeMask(match[0], match[1], lineLen)
			fmt.Printf("WordMask: %s\n", wordMask.Text(2))
			neighbourSymbols := getNeigbouringSymbols(symbols, index)
			fmt.Printf("neighbourSymbols: %s\n", neighbourSymbols.Text(2))
			checkResult := new(big.Int).And(&wordMask, &neighbourSymbols)
			fmt.Printf("checkResult: %s\n", checkResult.Text(2))
			if checkResult.Cmp(big.NewInt(0)) > 0 {
				fmt.Println("TAK", numberString)
				numberValue, err := strconv.Atoi(numberString)

				if err != nil {
					panic(err)
				}
				answer += numberValue
			} else {
				fmt.Println("NOPE", numberString)
			}
		}
	}

	return answer
}
func getRangeMask(start int, end int, lineLenght int) big.Int {
	//Extend index for neighbouring columns -
	maxIndex := lineLenght - 1
	extendedStart := max(start-1, 0)

	extendedEnd := min(end+1, lineLenght)
	result := new(big.Int)
	for i := extendedStart; i < extendedEnd; i++ {
		currentValue := new(big.Int).Lsh(big.NewInt(1), uint(maxIndex-i))
		// currentValue := 1 << (maxIndex - i)
		// fmt.Printf("My index to present %v : %010b\n", i, currentValue)
		result.Or(currentValue, result)
	}
	// fmt.Println("start:", extendedStart, "end:", extendedEnd, "Presentation:", fmt.Sprintf("%010b", result))
	return *result
}
func getNeigbouringSymbols(symbolsArray []big.Int, rowIndex int) big.Int {

	result := *new(big.Int).Set(&symbolsArray[rowIndex])
	fmt.Printf("Curr row %010b\n", result.Int64())
	//test
	for index, line := range symbolsArray {
		fmt.Printf("%d\t%010b\n", index, line.Uint64())
	}

	if rowIndex > 0 {
		result.Or(&result, &symbolsArray[rowIndex-1])
		// result |= symbolsArray[rowIndex-1]
		fmt.Printf("Prev row value \t%010b\n", symbolsArray[rowIndex-1].Int64())
		fmt.Printf("after Prev row \t%010b\n", result.Int64())
	}
	if rowIndex < len(symbolsArray)-1 {
		result.Or(&result, &symbolsArray[rowIndex+1])
		fmt.Printf("Next row value \t%010b\n", symbolsArray[rowIndex+1].Int64())
		fmt.Printf("Next row \t%010b\n", result.Int64())
	}
	return result
}

func partTwo(input []string) int {

	gearsRegex := regexp.MustCompile(`\*`)
	answer := 0
	for index, line := range input {
		gearMatches := gearsRegex.FindAllStringIndex(line, -1)
		for _, gearMatch := range gearMatches {
			gearMask := *new(big.Int).Lsh(big.NewInt(1), uint(len(line)-gearMatch[1]))
			var connectedNumbers []int
			if index > 0 {
				prevLine := input[index-1]
				connectedNumbers = append(connectedNumbers, getNumbersFromLine(gearMask, prevLine)...)
			}
			connectedNumbers = append(connectedNumbers, getNumbersFromLine(gearMask, line)...)
			if index <= len(input) {
				nextLine := input[index+1]
				connectedNumbers = append(connectedNumbers, getNumbersFromLine(gearMask, nextLine)...)
			}
			if len(connectedNumbers) != 2 {
				fmt.Println("Not a gear")
				continue
			}
			answer += connectedNumbers[0] * connectedNumbers[1]
		}
	}
	return answer
}
func getNumbersFromLine(gearMask big.Int, line string) []int {
	numbersRegexp := regexp.MustCompile("[0-9]+")

	fmt.Printf("Gear:\t%010b\n", gearMask.Uint64())
	numbersMatches := numbersRegexp.FindAllStringIndex(line, -1)
	var result []int
	for _, numberMatch := range numbersMatches {
		numberMask := getRangeMask(numberMatch[0], numberMatch[1], len(line))
		numberValue, err := strconv.Atoi(string(line[numberMatch[0]:numberMatch[1]]))
		fmt.Printf("number %v\t flag\t%010b\n", numberValue, numberMask.Uint64())
		checkResult := big.NewInt(0).And(&numberMask, &gearMask)

		if checkResult.Cmp(big.NewInt(0)) > 0 {
			fmt.Println("Is with cog ", numberValue)
			if err != nil {
				panic(err)
			}
			result = append(result, numberValue)
		}

	}
	return result
}
