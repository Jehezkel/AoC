package main

import (
	"fmt"
)

func main() {
	fmt.Println("Starting aoc application")
	fmt.Println(GCD(12, 18))
	fmt.Println(LCM(24,36))
}

func GCD(x, y int) int {
	for y > 0 {
		swapX := x
		x = y
		y = swapX % x

	}
	return x
}

func LCM(x, y int) int {
	return x * y / GCD(x, y)
}
