package main

import (
  "time"
  "fmt"
  "pegsim"
)

var gamesPlayed uint64
var solutions [][]*pegsim.Move
var startTime time.Time
var endTime time.Time

func search(gs *pegsim.GameState, moveStack []*pegsim.Move) error {
  if (gs.PegsRemaining() == 1) {
    moveStackCopy := make([]*pegsim.Move, len(moveStack))
    copy(moveStackCopy, moveStack)
    solutions = append(solutions, moveStackCopy)

    gamesPlayed++

    return nil
  }

  legalMoves := gs.LegalMoves()

  if (len(legalMoves) == 0) {
    gamesPlayed++
    return nil
  }

  for _, m := range legalMoves {
    nextState, err := gs.Apply(m)
    if err != nil {
      return err
    }
    moveStack = append(moveStack, m)

    err = search(nextState, moveStack)
    if err != nil {
      return err
    }

    moveStack = moveStack[:len(moveStack) - 1]
  }

  return nil
}

func main() {
  startTime = time.Now()
  gs := pegsim.NewGameState(5, pegsim.NewCoordinate(3, 2))

  err := search(gs, make([]*pegsim.Move, 0))
  if err != nil {
    fmt.Println("Program error: ", err)
    return
  }

  endTime = time.Now()

  fmt.Printf("Games Played:    %6d\n", gamesPlayed)
  fmt.Printf("Solutions Found: %6d\n", len(solutions))
  fmt.Printf("Time Elapsed:    %6dms\n", (endTime.Sub(startTime).Nanoseconds() / 1000000))
}

