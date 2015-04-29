package pegsim

import (
  "errors"
)

type GameState struct {
  rowCount uint32
  occupiedHoles []Coordinate
}

func NewGameState(rows uint32, emptyHole *Coordinate) *GameState {
  occupiedHoles := new([]Coordinate)
  for row := uint32(1); row <= rows; row++ {
    for hole := uint32(1); hole <= row; hole++ {
      peg := NewCoordinate(row, hole)
      if *peg != *emptyHole {
        occupiedHoles := append(*occupiedHoles, *peg)
      }
    }
  }
  return &GameState(rows, *occupiedHoles)
}

func (initialState *GameState) newGameStateWithAppliedMove(applyMe Move) *GameState {
  occupiedHoles := make([]Coordinate, len(initialState.occupiedHoles))
  copy(occupiedHoles, initialState.occupiedHoles)

  occupiedHoles, err := remove(occupiedHoles, applyMe.from())
  if err != Nil {
    return Nil, error("Move is not consistent with game state: 'from' hole was unoccupied.")
  }

  occupiedHoles, err = remove(occupiedHoles, applyMe.jumped())
  if err != Nil {
    return Nil, error("Move is not consistent with game state: jumped hole was unoccupied.")
  }

  occupiedHoles, err = remove(occupiedHoles, applyMe.to())
  if err != Nil {
    return Nil, error("Move is not consistent with game state: 'to' hole was occupied.")
  }

  if applyMe.to().row() > rowCount || applyMe.to().row() < 1 {
    error("Move is not legal because the 'to' hole does not exist: " + applyMe.getTo());
  }

  occupiedHoles = append(occupiedHoles, applyMe.to())

  newGs := &GameState(initialState.rowCount, occupiedHoles)
}

func (gs *GameState) LegalMoves() []Move {
  legalMoves := new([]Move)
  for _, c := range gs.occupiedHoles {
    possibleMoves := c.possibleMoves(gs.rowCount)
    for _, m := range possibleMoves {
      if contains(occupiedHoles, m.Jumped()) && contains(occupiedHoles, m.To()) {
        legalMoves := append(legalMoves, m)
      }
    }
  }
  return legalMoves
}

func (gs GameState) Apply(move Move) {
  return gs.newGameStateWithAppliedMove(move)
}

func (gs GameState) PegsRemaining() {
  return gs.occupiedHoles.Size()
}

func (gs GameState) String() {
  sb := bytes.Buffer
  sb.WriteString(fmt.Sprintf("Game with %d pegs:\n", gs.PegsRemaining()))
  for row := 1; row <= rowCount; row++ {
    indent := rowCount - row
    for i := 0; i < indent; i++ {
      sb.WriteString(" ")
    }
    for hole := 1; hole <= row; hole++ {
      if contains(gs.occupiedHoles, Coordinate.NewCoordinate(row, hole)) {
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

func remove(coords []Coordinate, toRemove Coordinate) ([]Coordinate, error) {
  idx := index(coords, toRemove)
  if idx >= 0 {
    return append(coords[:idx], coords[idx + 1:len(coords)]), Nil
  } else {
    return coords, errors.New("item to remove not found")
  }
}

func index(s []Coordinate, v Coordinate) int32 {
  for i, vv := range s {
    if v == vv {
      return i
    }
  }
  return -1
}
