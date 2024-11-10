package main

import (
	_ "embed"
	"errors"
	"flag"
	"fmt"
	"regexp"
	"slices"
	"strings"
	"sync"
)

//go:embed input.txt
var input string

func init() {
	input = strings.TrimRight(input, "\n")
	if len(input) == 0 {
		panic("empty input.txt")
	}
}

type TreeNode struct {
	value     string
	leftNode  string
	rightNode string
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

		// if err != nil {
		// 	panic(err)
		// }
		fmt.Println(answer)
	}

	fmt.Println("Output:", answer)
}

func partOne(input []string) int {
	pathArray := []rune(strings.TrimSpace(input[0]))
	nodesRegex := regexp.MustCompile("[A-Z]{3}")
	treeMap := map[string]TreeNode{}
	answer := 0
	for i := 2; i < len(input); i++ {
		line := input[i]
		nodesMatches := nodesRegex.FindAllString(line, 3)
		parentNodeVal, leftNodeVal, rightNodeVal := nodesMatches[0], nodesMatches[1], nodesMatches[2]
		treeMap[parentNodeVal] = TreeNode{value: parentNodeVal, leftNode: leftNodeVal, rightNode: rightNodeVal}
	}
	currentNodeVal := "AAA"
	desiredDestVal := "ZZZ"
	pathLen := len(pathArray)
	fmt.Println(pathArray)
	for i := 0; true; i++ {
		currentDir := pathArray[i%pathLen]
		fmt.Println("Current step:", i, "Dir", string(currentDir))
		currentNode := treeMap[currentNodeVal]
		switch string(currentDir) {
		case "L":
			currentNodeVal = currentNode.leftNode
		case "R":
			currentNodeVal = currentNode.rightNode
		default:
			panic("Wrong step")
		}
		if currentNodeVal == desiredDestVal {
			answer = i + 1
			break
		}
	}
	return answer
}

func partTwo(input []string) int {
	pathArray := []rune(strings.TrimSpace(input[0]))
	nodesRegex := regexp.MustCompile("[0-9A-Z]{3}")
	treeMap := map[string]TreeNode{}
	for i := 2; i < len(input); i++ {
		line := input[i]
		nodesMatches := nodesRegex.FindAllString(line, 3)
		parentNodeVal, leftNodeVal, rightNodeVal := nodesMatches[0], nodesMatches[1], nodesMatches[2]
		treeMap[parentNodeVal] = TreeNode{value: parentNodeVal, leftNode: leftNodeVal, rightNode: rightNodeVal}
	}

	startingPoints := []string{}
	endingPoints := []string{}
	for key := range treeMap {
		determinant := key[2:3]
		if determinant == "Z" {
			endingPoints = append(endingPoints, key)
		} else if determinant == "A" {
			startingPoints = append(startingPoints, key)
		}

	}
	pathLen := len(pathArray)
	fmt.Println(pathArray)
	fmt.Println("starting:", startingPoints, "endingPoints", endingPoints)
	currentNodeVals := startingPoints
	runningGhosts := len(startingPoints)

	results := make([]int, runningGhosts)
	for j := 0; j < runningGhosts; j++ {
		for {
			currentDir := pathArray[results[j]%pathLen]
			currentNodeVal := currentNodeVals[j]
			fmt.Println("Current step:", results[j], "Dir", string(currentDir))
			currentNode := treeMap[currentNodeVal]
			switch string(currentDir) {
			case "L":
				currentNodeVals[j] = currentNode.leftNode
			case "R":
				currentNodeVals[j] = currentNode.rightNode
			default:
				panic("Wrong step")
			}
			if slices.Contains(endingPoints, currentNodeVal) {
				break
			}
			results[j]++
		}
	}
	answer := LCM(results[0], results[1:])
	return answer
}
func GCD(a, b int) int {

	for b != 0 {
		t := b
		b = a % b
		a = t
	}
	return a
}

func LCM(first int, integers []int) int {
	result := first * integers[0] / GCD(first, integers[0])
	for i := 1; i < len(integers); i++ {
		result = LCM(result, []int{integers[i]})
	}
	return result
}
