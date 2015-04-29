package pegsim

type Move struct {
  from Coordinate
  jumped Coordinate
  to Coordinate
}

func NewMove(from *Coordinate, jumped *Coordinate, to *Coordinate) *Move {
  return &Move(from, jumped, to)
}

func (m Move) From() *Move {
  return m.from
}

func (m Move) Jumped() *Move {
  return m.jumped
}

func (m Move) To() *Move {
  return m.to
}

func (m Move) String() {
  return from + " -> " + jumped + " -> " + to
}

// don't need to define equals and hashcode; golang does this implicitly for structs
