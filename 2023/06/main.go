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

	var answer uint64
	inputArr := strings.Split(input, "\n")
	if part == 1 {
		answer = partOne(inputArr)
	} else {
		answer = partTwo(inputArr)
	}

	fmt.Println("Output:", answer)
}

type RaceData struct {
	distance          int
	timeToBeat        int
	minTimeOfCharging int
	maxTimeOfCharging int
	waysToBeat       uint64 
}

func partOne(input []string) uint64 {
	timeFields := strings.Fields(input[0])
	distanceFields := strings.Fields(input[1])
	raceDataArray := []RaceData{}
	result := uint64(1)
	for i := 1; i < len(timeFields); i++ {
		currRaceData := RaceData{distance: castToInt(distanceFields[i]), timeToBeat: castToInt(timeFields[i]), waysToBeat: 0}
		raceDataArray = append(raceDataArray, currRaceData)
	}
	for _, race := range raceDataArray {
		maxUnits := race.timeToBeat - 1
		for speed := 1; speed < maxUnits; speed++ {
			time := race.distance / speed
			if time+speed < race.timeToBeat {
				fmt.Println("Speed", speed, "time", time)
				race.waysToBeat++
			}
		}
		fmt.Println(race)
		result *= race.waysToBeat
	}
	fmt.Println(raceDataArray)
	return result
}
func castToInt(str string) int {
	if result, err := strconv.Atoi(str); err == nil {
		return result
	} else {
		panic(err)
	}
}

func partTwo(input []string) uint64 {
	timeFields := strings.Fields(input[0])
	distanceFields := strings.Fields(input[1])
	raceDataArray := []RaceData{}
	result := uint64(1)
	totalDistance, totalTime := "", ""
	for i := 1; i < len(timeFields); i++ {
		totalDistance += distanceFields[i]
		totalTime += timeFields[i]
	}
	currRaceData := RaceData{distance: castToInt(totalDistance), timeToBeat: castToInt(totalTime), waysToBeat: 0}
	raceDataArray = append(raceDataArray, currRaceData)
	fmt.Println("dist", totalDistance, "totalTime", totalTime)
	for _, race := range raceDataArray {
		maxUnits := race.timeToBeat - 1
		for speed := 1; speed < maxUnits; speed++ {
			time := race.distance / speed
			if time+speed < race.timeToBeat {
				fmt.Println("Speed", speed, "time", time)
				race.waysToBeat++
			}
		}
		fmt.Println(race)
		result *= race.waysToBeat
	}
	fmt.Println(raceDataArray)
	return result
}
