package main

import (
	_ "embed"
	"flag"
	"fmt"
	"log"
	"os"
	"path/filepath"
	"time"
)

//go:embed tmpl/main.go
var mainFile []byte

//go:embed tmpl/input.txt
var inputFile []byte

func main() {
	today := time.Now()
	day := flag.Int("day", today.Day(), "day number to fetch, 1-25")
	year := flag.Int("year", today.Year(), "AOC year")
	flag.Parse()
	run(*day, *year)

}
func run(day int, year int) {
	fmt.Printf("Executing skeleton for day %d of %d \n", day, year)
	dstDir := createDirectory(day, year)
	copyTemplate(dstDir)

}

func createDirectory(day int, year int) string {
	dirToCreate := fmt.Sprintf("%d/%02d", year, day)
	fmt.Printf("Creating directory %s\n", dirToCreate)
	if err := os.MkdirAll(dirToCreate, os.ModePerm); err != nil {
		log.Fatal(err)
	}
	return dirToCreate
}
func copyTemplate(newDir string) {
	dstMainFile := filepath.Join(newDir, "main.go")
	dstInputFile := filepath.Join(newDir, "input.txt")

	copyFile(mainFile, dstMainFile)
	copyFile(inputFile, dstInputFile)

}
func copyFile(fileContent []byte, dst string) {
	destination, err := os.Create(dst)

	if err != nil {
		panic(err)
	}
	defer destination.Close()
	if err := os.WriteFile(dst, fileContent, 0644); err != nil {
		panic(err)
	}

}
