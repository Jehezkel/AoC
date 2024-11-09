package main

import (
	"cmp"
	_ "embed"
	"flag"
	"fmt"
	"slices"
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

// Pogrupowac karty
// Przeanalizowac typ reki
// zgrupowane karty
// Five of a kind, where all five cards have the same label: AAAAA
// Four of a kind, where four cards have the same label and one card has a different label: AA8AA
// Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
// Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
// Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
// One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
// High card, where all cards' labels are distinct: 23456
// zmienic karty na wartosci

type HandScoreAndBid struct {
	handScore int
	bid       int
	cards     string
}

func partOne(input []string) int {
	deckOfCards := []rune{'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'}

	slices.Reverse(deckOfCards)
	// for _, card := range deckOfCards {
	// 	fmt.Println(string(card), slices.Index(deckOfCards, card))
	// }
	handsScoreList := []HandScoreAndBid{}

	for _, line := range input {
		lineFields := strings.Fields(line)

		hand := lineFields[0]
		groupedCards := groupByCard([]rune(hand))

		handTypeScore := 0
		cardScoreString := ""
		for _, card := range hand {
			count := groupedCards[card]
			valueOfCard := fmt.Sprintf("%02d", slices.Index(deckOfCards, card))
			fmt.Println("Card:", string(card), "count", count, "card value", valueOfCard)
			switch count {
			case 5:
				handTypeScore += 1000
			case 4:
				handTypeScore += 100
			case 3:
				handTypeScore += 10
			case 2:
				handTypeScore += 1
			}
			fmt.Println("Przed", cardScoreString)
			cardScoreString += valueOfCard
			fmt.Println("Po", cardScoreString)

		}
		// fmt.Println(groupedCards)
		fmt.Println(line)
		fmt.Println(cardScoreString)
		// fmt.Println(handTypeScore, cardScoreString)
		totalScore := castToInt(fmt.Sprintf("%v%s", handTypeScore, cardScoreString))
		fmt.Println(totalScore)
		currEntry := HandScoreAndBid{
			handScore: totalScore, bid: castToInt(lineFields[1]), cards: lineFields[0]}
		handsScoreList = append(handsScoreList, currEntry)

	}
	slices.SortFunc(handsScoreList, func(prev, curr HandScoreAndBid) int {
		return cmp.Compare(prev.handScore, curr.handScore)
	})
	answer := 0
	for i := 0; i < len(handsScoreList); i++ {
		fmt.Println(i+1, handsScoreList[i])
		answer += handsScoreList[i].bid * (i + 1)

	}
	return answer
}
func groupByCard(cards []rune) map[rune]int {
	groupedCards := map[rune]int{}
	for _, card := range cards {
		groupedCards[card]++
		fmt.Printf("%s: %v\n", string(card), groupedCards[card])
	}
	return groupedCards
}
func castToInt(str string) int {
	if result, err := strconv.Atoi(str); err == nil {
		return result
	} else {
		panic(err)
	}
}

func partTwo(input []string) int {
	deckOfCards := []rune{'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'}

	slices.Reverse(deckOfCards)
	// for _, card := range deckOfCards {
	// 	fmt.Println(string(card), slices.Index(deckOfCards, card))
	// }
	handsScoreList := []HandScoreAndBid{}

	for _, line := range input {
		lineFields := strings.Fields(line)

		hand := lineFields[0]
		groupedCards := groupByCard([]rune(hand))

		handTypeScore := 0
		cardScoreString := ""
		for _, card := range hand {
			cardValue := slices.Index(deckOfCards, card)
			cardScoreString += fmt.Sprintf("%02d", cardValue)
		}
		maxCount := 0
		for card, count := range groupedCards {
			if count > maxCount && card != 'J' {
				maxCount = count
			}
		}
		jBonus := groupedCards['J']
		for card, count := range groupedCards {
			if len(groupedCards) > 1 && card == 'J' {
				continue
			}
			if count == maxCount && card != 'J' {
				count += jBonus
				jBonus = 0
			}
			switch count {
			case 5:
				handTypeScore += 1000
			case 4:
				handTypeScore += 100
			case 3:
				handTypeScore += 10
			case 2:
				handTypeScore += 1
			}
		}

		totalScore := castToInt(fmt.Sprintf("%v%s", handTypeScore, cardScoreString))
		fmt.Println(totalScore)
		currEntry := HandScoreAndBid{
			handScore: totalScore, bid: castToInt(lineFields[1]), cards: lineFields[0]}
		handsScoreList = append(handsScoreList, currEntry)

	}
	slices.SortFunc(handsScoreList, func(prev, curr HandScoreAndBid) int {
		return cmp.Compare(prev.handScore, curr.handScore)
	})
	answer := 0
	for i := 0; i < len(handsScoreList); i++ {
		fmt.Println(i+1, handsScoreList[i])
		answer += handsScoreList[i].bid * (i + 1)

	}
	return answer
}
