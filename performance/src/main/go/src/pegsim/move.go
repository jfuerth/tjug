package pegsim
import "fmt"

// TODO hold all members by reference?
type Move struct {
  from Coordinate
  jumped Coordinate
  to Coordinate
}

func NewMove(from *Coordinate, jumped *Coordinate, to *Coordinate) Move {
  return Move{*from, *jumped, *to}
}

func (m Move) From() *Coordinate {
  return &m.from
}

func (m Move) Jumped() *Coordinate {
  return &m.jumped
}

func (m Move) To() *Coordinate {
  return &m.to
}

func (m Move) String() string {
  return fmt.Sprint(m.from, " -> ", m.jumped, " -> ", m.to)
}

// don't need to define equals and hashcode; golang does this implicitly for structs
