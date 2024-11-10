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

	answer :=0
	for _, line := range input {

		numbersStrings := strings.Fields(line)
		numbers := []int{}
		for _, numberString := range numbersStrings {
			numbers = append(numbers, castToInt(numberString))
		}

		newNumbersArrays := [][]int{}
		newNumbersArrays = append(newNumbersArrays, numbers)
		for {
			newNumbers := []int{}
			isNullRow := true
			for i := 1; i < len(numbers); i++ {
				diff := numbers[i] - numbers[i-1]
				newNumbers = append(newNumbers, diff)
				if diff != 0 {
					isNullRow = false
				}

			}
			fmt.Println(newNumbers)
			numbers = newNumbers
			if isNullRow {
				break
			}
			newNumbersArrays = append(newNumbersArrays, newNumbers)
		}
		prevValue := 0
		fmt.Println(newNumbersArrays)
		for i := len(newNumbersArrays)-1; i >= 0; i-- {
			currArray := newNumbersArrays[i]
			prevValue += currArray[len(currArray)-1]
		}
		fmt.Println(prevValue)
		answer+=prevValue
	}
	return answer
}

func partTwo(input []string) int {

	answer :=0
	for _, line := range input {

		numbersStrings := strings.Fields(line)
		numbers := []int{}
		for _, numberString := range numbersStrings {
			numbers = append(numbers, castToInt(numberString))
		}

		newNumbersArrays := [][]int{}
		newNumbersArrays = append(newNumbersArrays, numbers)
		for {
			newNumbers := []int{}
			isNullRow := true
			for i := 1; i < len(numbers); i++ {
				diff := numbers[i] - numbers[i-1]
				newNumbers = append(newNumbers, diff)
				if diff != 0 {
					isNullRow = false
				}

			}
			fmt.Println(newNumbers)
			numbers = newNumbers
			if isNullRow {
				break
			}
			newNumbersArrays = append(newNumbersArrays, newNumbers)
		}
		prevValue := 0
		fmt.Println(newNumbersArrays)
		for i := len(newNumbersArrays)-1; i >= 0; i-- {
			currArray := newNumbersArrays[i]
			prevValue = currArray[0]-prevValue
		}
		fmt.Println(prevValue)
		answer+=prevValue
	}
	return answer

}
func castToInt(str string) int {
	if val, err := strconv.Atoi(str); err == nil {
		return val
	} else {
		panic(err)
	}
}
