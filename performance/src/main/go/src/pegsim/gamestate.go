package pegsim

import (
  "errors"
  "bytes"
  "fmt"
  "runtime/debug"
)

type GameState struct {
  rowCount int
  occupiedHoles []Coordinate
}

func NewGameState(rows int, emptyHole *Coordinate) *GameState {
  occupiedHoles := []Coordinate{}
  for row := 1; row <= rows; row++ {
    for hole := 1; hole <= row; hole++ {
      peg := NewCoordinate(row, hole)
      if *peg != *emptyHole {
        occupiedHoles = append(occupiedHoles, *peg)
      }
    }
  }
  return &GameState{rows, occupiedHoles}
}

func (initialState *GameState) newGameStateWithAppliedMove(applyMe *Move) (*GameState, error) {
  occupiedHoles := make([]Coordinate, len(initialState.occupiedHoles))
  copy(occupiedHoles, initialState.occupiedHoles)

  occupiedHoles, err := remove(occupiedHoles, applyMe.From())
  if err != nil {
    debug.PrintStack()
    return nil, errors.New("Move is not consistent with game state: 'from' hole was unoccupied.")
  }

  occupiedHoles, err = remove(occupiedHoles, applyMe.Jumped())
  if err != nil {
    debug.PrintStack()
    return nil, errors.New("Move is not consistent with game state: jumped hole was unoccupied.")
  }

  if contains(occupiedHoles, *applyMe.To()) {
    debug.PrintStack()
    fmt.Println("error when applying", applyMe, "to", initialState)
    return nil, errors.New("Move is not consistent with game state: 'to' hole was occupied.")
  }

  if applyMe.To().Row() > initialState.rowCount || applyMe.To().Row() < 1 {
    errors.New(fmt.Sprintf("Move is not legal because the 'to' hole does not exist: %s", applyMe.To()));
  }

  occupiedHoles = append(occupiedHoles, *applyMe.To())

  return &GameState{initialState.rowCount, occupiedHoles}, nil
}

func (gs *GameState) LegalMoves() []*Move {
  legalMoves := make([]*Move, 0)
  for _, c := range gs.occupiedHoles {
    possibleMoves := c.possibleMoves(gs.rowCount)
    for _, m := range possibleMoves {
      if contains(gs.occupiedHoles, *m.Jumped()) && !contains(gs.occupiedHoles, *m.To()) {
        legalMoves = append(legalMoves, m)
      }
    }
  }
  return legalMoves
}

func (gs *GameState) Apply(move *Move) (*GameState, error) {
  return gs.newGameStateWithAppliedMove(move)
}

func (gs *GameState) PegsRemaining() int {
  return len(gs.occupiedHoles)
}

func (gs *GameState) String() string {
  sb := bytes.Buffer{}
  sb.WriteString(fmt.Sprintf("Game with %d pegs:\n", gs.PegsRemaining()))
  for row := 1; row <= gs.rowCount; row++ {
    indent := gs.rowCount - row
    for i := 0; i < indent; i++ {
      sb.WriteString(" ")
    }
    for hole := 1; hole <= row; hole++ {
      if contains(gs.occupiedHoles, *NewCoordinate(row, hole)) {
        sb.WriteString(" *")
      } else {
        sb.WriteString(" O")
      }
    }
    sb.WriteString("\n")
  }
  return sb.String()
}

func contains(s []Coordinate, v Coordinate) bool {
  return index(s, v) >= 0
}

func remove(coords []Coordinate, toRemove *Coordinate) ([]Coordinate, error) {
  idx := index(coords, *toRemove)
  if idx >= 0 {
    return append(coords[:idx], coords[idx + 1:len(coords)]...), nil
  } else {
    return coords, errors.New("item to remove not found")
  }
}

func index(s []Coordinate, v Coordinate) int {
  for i, vv := range s {
    if v == vv {
      return i
    }
  }
  return -1
}
