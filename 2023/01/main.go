package main

import (
	_ "embed"
	"flag"
	"fmt"
	"regexp"
	"slices"
	"strconv"
	"strings"
	"unicode"
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
	sum := 0
	for _, line := range input {

		var firstNum *rune
		var lastNum *rune

		for _, char := range line {
			isNumber := unicode.IsNumber(char)
			if isNumber {
				if firstNum == nil {
					firstNum = &char
				}
				lastNum = &char
			}
		}

		combinedStringValue := fmt.Sprintf("%c%c", *firstNum, *lastNum)
		result, err := strconv.Atoi(combinedStringValue)

		if err != nil {
			fmt.Printf("Error on conversion to int on line %s\n", line)
			panic(err)
		}
		sum += result
	}
	fmt.Println("Result of partOne:", sum)
	return sum
}

func partTwo(input []string) int {
	sum := 0
	for _, line := range input {
		var firstNum *rune
		var lastNum *rune
		for index, value := range line {
			var currCipher *rune
			if unicode.IsNumber(value) {
				currCipher = &value
			} else {

				lineSubstring := line[index:]
				receivedVal, err := checkIfContainsTextValue(lineSubstring)
				if err == nil {
					currCipher = &receivedVal
				}
			}
			if currCipher != nil {
				if firstNum == nil {
					firstNum = currCipher
				}
				lastNum = currCipher
			}

		}
		stringValue := fmt.Sprintf("%c%c", *firstNum, *lastNum)
		resultValue, err := strconv.Atoi(stringValue)

		if err != nil {
			panic(err)
		}
		recValue := resultValue
		sum += recValue
	}
	return sum
}

func checkIfContainsTextValue(line string) (rune, error) {
	number_words := []string{"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}
	numbersJoined := strings.Join(number_words, "|")
	expr := fmt.Sprintf("^(%s)", numbersJoined)

	r, err := regexp.Compile(expr)

	if err != nil {
		panic(err)
	}
	findResult := r.Find([]byte(line))
	if findResult != nil {
		numericValue := slices.Index(number_words, string(findResult)) + 1
		return rune('0' + numericValue), nil
	}
	return 0, fmt.Errorf("value not found in string %s", line)

}
