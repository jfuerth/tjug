package pegsim
import "fmt"

type Coordinate struct {
  hole int
  row int
}

func NewCoordinate(row int, hole int) *Coordinate {
  if hole < 1 {
    return nil
  }
  if hole > row {
    return nil
  }
  return &Coordinate{hole, row} 
}

func (c *Coordinate) Row() int {
  return c.row
}

func (c *Coordinate) Hole() int {
  return c.hole
}

func (c *Coordinate) possibleMoves(rowCount int) []*Move {
  moves := []*Move{}

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
  return fmt.Sprint("r", c.row, "h", c.hole);
}

// don't need to define equals and hashcode; golang does this implicitly for structs
