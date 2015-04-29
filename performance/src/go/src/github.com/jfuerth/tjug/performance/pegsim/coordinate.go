package pegsim

type Coordinate struct {
  hole uint32
  row uint32
}

func NewCoordinate(row uint32, hole uint32) *Coordinate {
  c := new(Coordinate)
  if hole < 1 {
    return nil
  }
  if hole > row {
    return nil
  }
  return &Coordinate{hole, row} 
}

func (c *Coordinate) Row() uint32 {
  return c.row
}

func (c *Coordinate) Hole() uint32 {
  return c.hole
}

func (c *Coordinate) possibleMoves(rowCount uint32) []Move {
  moves := new([]Move)

  // upward (needs at least 2 rows above)
  if c.row >= 3 {

    // up-left
    if c.hole >= 3 {
      moves = append(
          moves,
          NewMove(
              c,
              NewCoordinate(c.row - 1, c.hole - 1),
              NewCoordinate(c.row - 2, c.hole - 2)))
    }

    // up-right
    if c.row - c.hole >= 2 {
      moves = append(
          moves,
          NewMove(
              c,
              NewCoordinate(c.row - 1, c.hole),
              NewCoordinate(c.row - 2, c.hole)))
    }
  }

  // leftward (needs at least 2 pegs to the left)
  if c.hole >= 3 {
    moves = append(
        moves,
        NewMove(
            c,
            NewCoordinate(c.row, c.hole - 1),
            NewCoordinate(c.row, c.hole - 2)))
  }

  // rightward (needs at least 2 holes to the right)
  if c.row - c.hole >= 2 {
    moves = append(
        moves,
        NewMove(
            c,
            NewCoordinate(c.row, c.hole + 1),
            NewCoordinate(c.row, c.hole + 2)))
  }

  // downward (needs at least 2 rows below)
  if rowCount - c.row >= 2 {

    // down-left (always possible when there are at least 2 rows below)
    moves = append(
        moves,
        NewMove(
            c,
            NewCoordinate(c.row + 1, c.hole),
            NewCoordinate(c.row + 2, c.hole)))

    // down-right (always possible when there are at least 2 rows below)
    moves = append(
        moves,
        NewMove(
            c,
            NewCoordinate(c.row + 1, c.hole + 1),
            NewCoordinate(c.row + 2, c.hole + 2)))
  }

  return moves
}

func (c *Coordinate) String() string {
  return "r" + row + "h" + hole;
}

// don't need to define equals and hashcode; golang does this implicitly for structs
