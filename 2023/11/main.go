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

	inputArr := strings.Split(input, "\n")
	var answer uint64
	if part == 1 {
		answer = partOne(inputArr)
	} else {
		answer = partTwo(inputArr)
	}

	fmt.Println("Output:", answer)
}

type point struct {
	galaxyNum int
	row       int
	col       int
}

func partOne(input []string) uint64 {
	numOfCols := len(input[0])
	numOfRows := len(input)
	isGalaxyRow := make([]bool, numOfRows)
	isGalaxyCol := make([]bool, numOfCols)

	galaxies := []point{}
	for i := 0; i < len(input); i++ {
		line := input[i]
		for j := 0; j < len(input[0]); j++ {

			currSymbol := string(line[j])
			if currSymbol == "." {
				continue
			}
			currGalaxy := point{row: i, col: j, galaxyNum: len(galaxies) + 1}
			galaxies = append(galaxies, currGalaxy)
			isGalaxyCol[j] = true
			isGalaxyRow[i] = true
		}
	}
	answer := 0
	pairs := 0
	for index, galaxy := range galaxies {

		for i := index + 1; i < len(galaxies); i++ {
			pairs++
			currDestGalaxy := galaxies[i]

			distance := 0
			minRow := min(galaxy.row, currDestGalaxy.row)
			maxRow := max(galaxy.row, currDestGalaxy.row)
			fmt.Println("Row", minRow, maxRow)
			for rowStep := minRow+1; rowStep <= maxRow; rowStep++ {
				if isGalaxyRow[rowStep] {
					distance++
				} else {
					distance+=2
				}
			}
			minCol := min(galaxy.col, currDestGalaxy.col)
			maxCol := max(galaxy.col, currDestGalaxy.col)
			fmt.Println("col", minCol, maxCol)
			for colStep := minCol+1; colStep <=maxCol; colStep++ {

				if isGalaxyCol[colStep] {
					distance++
				} else {
					distance+=2
				}
			}
			fmt.Println("From", galaxy.galaxyNum, "To", currDestGalaxy.galaxyNum, "dist", distance)
			answer += distance
		}
	}
	fmt.Println(pairs)
	uintResult := uint64(answer)
	return uintResult
}

func partTwo(input []string) uint64{
	numOfCols := len(input[0])
	numOfRows := len(input)
	isGalaxyRow := make([]bool, numOfRows)
	isGalaxyCol := make([]bool, numOfCols)

	galaxies := []point{}
	for i := 0; i < len(input); i++ {
		line := input[i]
		for j := 0; j < len(input[0]); j++ {

			currSymbol := string(line[j])
			if currSymbol == "." {
				continue
			}
			currGalaxy := point{row: i, col: j, galaxyNum: len(galaxies) + 1}
			galaxies = append(galaxies, currGalaxy)
			isGalaxyCol[j] = true
			isGalaxyRow[i] = true
		}
	}
	answer := uint64(0)
	pairs := 0
	for index, galaxy := range galaxies {

		for i := index + 1; i < len(galaxies); i++ {
			pairs++
			currDestGalaxy := galaxies[i]

			distance := uint64(0)
			minRow := min(galaxy.row, currDestGalaxy.row)
			maxRow := max(galaxy.row, currDestGalaxy.row)
			fmt.Println("Row", minRow, maxRow)
			for rowStep := minRow+1; rowStep <= maxRow; rowStep++ {
				if isGalaxyRow[rowStep] {
					distance++
				} else {
					distance+=1000000
				}
			}
			minCol := min(galaxy.col, currDestGalaxy.col)
			maxCol := max(galaxy.col, currDestGalaxy.col)
			fmt.Println("col", minCol, maxCol)
			for colStep := minCol+1; colStep <=maxCol; colStep++ {

				if isGalaxyCol[colStep] {
					distance++
				} else {
					distance+=1000000
				}
			}
			fmt.Println("From", galaxy.galaxyNum, "To", currDestGalaxy.galaxyNum, "dist", distance)
			answer += distance
		}
	}
	fmt.Println(pairs)
	return answer
}
