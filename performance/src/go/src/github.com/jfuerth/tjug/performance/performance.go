package main

import (
  "time"
  "github.com/jfuerth/tjug/performance/pegsim"
)

var gamesPlayed uint64
var solutions [][]Move
var startTime Time
var endTime Time

func search(gs GameState, moveStack []Move) {
  if (gs.PegsRemaining() == 1) {
    moveStackCopy := make([]Move, len(moveStack))
    copy(moveStackCopy, moveStack)
    solutions = append(solutions, moveStackCopy)

    gamesPlayed++

    return
  }

  legalMoves := gs.legalMoves()

  if (len(legalMoves) == 0) {
    gamesPlayed++
    return
  }

  for _,m := range legalMoves {
    nextState := gs.apply(m)
    moveStack = append(moveStack, m)
    search(nextState, moveStack)
    moveStack = moveStack[:len(moveStack - 1)]
  }
}

func main() {
  startTime = time.Now()
  gs := GameState.new(5, Coordinate.new(3, 2))
  search(gs, []Move)
  endTime = time.Now()

  fmt.Printf("Games Played:    %6d\n", gamesPlayed)
  fmt.Printf("Solutions Found: %6d\n", solutions.size())
  fmt.Printf("Time Elapsed:    %6dms\n", (endTime - startTime) / 10000000)
}

