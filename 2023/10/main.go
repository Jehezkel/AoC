package main

import (
	_ "embed"
	"flag"
	"fmt"
	"slices"
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
	inputArry := strings.Split(input, "\n")
	if part == 1 {
		answer = partOne(inputArry)
	} else {
		answer = partTwo(inputArry)
	}

	fmt.Println("Output:", answer)
}

type MapPoint struct {
	row    int
	col    int
	pointA *MapPoint
	pointB *MapPoint
	symbol string
	steps  int
}

func (point *MapPoint) IsConnected(otherPoint *MapPoint) bool {
	return otherPoint.pointA == point || otherPoint.pointB == point
}

//	1	2	3	4	5
//
// 1
func getPointerOrNull(mapPoints [][]*MapPoint, row, col int) *MapPoint {
	fmt.Println("col", col, "row", row)
	if row >= len(mapPoints) || col >= len(mapPoints[0]) || row < 0 || col < 0 {
		return nil
	}
	return mapPoints[row][col]
}
func partOne(input []string) int {

	numOfLines := len(input)
	mapPoints := make([][]*MapPoint, numOfLines)
	lineLen := len(input[0])
	for i := 0; i < numOfLines; i++ {
		mapPoints[i] = make([]*MapPoint, lineLen)
		for j := 0; j < lineLen; j++ {
			mapPoints[i][j] = &MapPoint{col: j, row: i}

		}
	}
	var startingPoint *MapPoint
	for i, line := range input {
		for j, char := range line {

			mapPoints[i][j].symbol = string(char)
			switch char {
			case '|':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case '-':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j-1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j+1)
			case 'L':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j+1)
			case 'J':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j-1)
			case '7':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j-1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case 'F':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j+1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case 'S':
				startingPoint = mapPoints[i][j]
			}
		}

	}
	fmt.Println(startingPoint)
	//BFS
	isVisited := make(map[*MapPoint]bool)
	isVisited[startingPoint] = true
	var toBeVisited Queue
	startingPoint.steps = 0
	if startingPoint.col > 0 {
		toBeAdded := mapPoints[startingPoint.row][startingPoint.col-1]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.col < len(mapPoints[0])-1 {
		toBeAdded := mapPoints[startingPoint.row][startingPoint.col+1]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.row > 0 {
		toBeAdded := mapPoints[startingPoint.row-1][startingPoint.col]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.row < len(mapPoints)-1 {
		toBeAdded := mapPoints[startingPoint.row+1][startingPoint.col]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	answer := 0
	for !toBeVisited.IsEmpty() {
		currElement := toBeVisited.Dequeue()
		fmt.Println(currElement.symbol, currElement.row, currElement.col)
		fmt.Println(currElement.pointA, "x", currElement.pointB)
		if currElement.pointA != nil && !isVisited[currElement.pointA] {
			toBeVisited.Enqueue(currElement.pointA)
		}
		if currElement.pointB != nil && !isVisited[currElement.pointB] {
			toBeVisited.Enqueue(currElement.pointB)
		}
		isVisited[currElement] = true

		if currElement.pointA != nil && isVisited[currElement.pointA] {

			currElement.steps = currElement.pointA.steps + 1
		}
		if currElement.pointB != nil && isVisited[currElement.pointB] {

			currElement.steps = currElement.pointB.steps + 1
		}
		if currElement.steps > answer {
			answer = currElement.steps
		}

	}
	return answer
}

func partTwo(input []string) int {

	numOfLines := len(input)
	mapPoints := make([][]*MapPoint, numOfLines)
	lineLen := len(input[0])
	for i := 0; i < numOfLines; i++ {
		mapPoints[i] = make([]*MapPoint, lineLen)
		for j := 0; j < lineLen; j++ {
			mapPoints[i][j] = &MapPoint{col: j, row: i}

		}
	}
	var startingPoint *MapPoint
	for i, line := range input {
		for j, char := range line {

			mapPoints[i][j].symbol = string(char)
			switch char {
			case '|':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case '-':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j-1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j+1)
			case 'L':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j+1)
			case 'J':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i-1, j)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i, j-1)
			case '7':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j-1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case 'F':
				mapPoints[i][j].pointA = getPointerOrNull(mapPoints, i, j+1)
				mapPoints[i][j].pointB = getPointerOrNull(mapPoints, i+1, j)
			case 'S':
				startingPoint = mapPoints[i][j]
			}
		}

	}
	fmt.Println(startingPoint)
	//BFS
	isVisited := make(map[*MapPoint]bool)
	isVisited[startingPoint] = true
	var toBeVisited Queue
	if startingPoint.col > 0 {
		toBeAdded := mapPoints[startingPoint.row][startingPoint.col-1]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.col < len(mapPoints[0])-1 {
		toBeAdded := mapPoints[startingPoint.row][startingPoint.col+1]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.row > 0 {
		toBeAdded := mapPoints[startingPoint.row-1][startingPoint.col]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	if startingPoint.row < len(mapPoints)-1 {
		toBeAdded := mapPoints[startingPoint.row+1][startingPoint.col]
		if startingPoint.IsConnected(toBeAdded) {
			toBeVisited.Enqueue(toBeAdded)
		}
	}
	answer := 0
	for !toBeVisited.IsEmpty() {
		currElement := toBeVisited.Dequeue()
		fmt.Println(currElement.symbol, currElement.row, currElement.col)
		fmt.Println(currElement.pointA, "x", currElement.pointB)
		if currElement.pointA != nil && !isVisited[currElement.pointA] {
			toBeVisited.Enqueue(currElement.pointA)
		}
		if currElement.pointB != nil && !isVisited[currElement.pointB] {
			toBeVisited.Enqueue(currElement.pointB)
		}
		isVisited[currElement] = true
	}

	for i := 0; i < len(input); i++ {
		for j := 0; j < len(input[0]); j++ {
			currElement := mapPoints[i][j]
			if isVisited[currElement] {
				continue
			}
			crossedIntersections := countRayIntersections(mapPoints[i], j, isVisited)
			isInsideLoop := crossedIntersections%2 == 1
			if isInsideLoop {
				answer++
			}
		}
	}

	return answer
}

// https://rosettacode.org/wiki/Ray-casting_algorithm
func countRayIntersections(line []*MapPoint, col int, isVisited map[*MapPoint]bool) int {
	charactersToTrack := []string{"L", "J", "|", "S"}
	intersections := 0
	for i := 0; i < col; i++ {
		currElement := line[i]
		isTrackedElement := slices.Contains(charactersToTrack, currElement.symbol)
		isPartOfLoop := isVisited[currElement]
		if isTrackedElement && isPartOfLoop {
			intersections++
		}
	}
	return intersections
}

type Queue struct {
	List []*MapPoint
}

func (q *Queue) Enqueue(element *MapPoint) {
	q.List = append(q.List, element)
}
func (q *Queue) Dequeue() *MapPoint {
	dequeuedItem := q.List[0]
	q.List = q.List[1:]
	return dequeuedItem
}
func (q *Queue) IsEmpty() bool {
	return len(q.List) == 0
}
