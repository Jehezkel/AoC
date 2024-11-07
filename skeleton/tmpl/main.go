package main

import (
	_ "embed"
	"flag"
	"fmt"
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
	if part == 1 {
		answer = partOne(input)
	} else {
		answer = partTwo(input)
	}

	fmt.Println("Output:", answer)
}

func partOne(input string) int {
	panic("unimplemented")
}

func partTwo(input string) int {
	panic("unimplemented")
}

