using System;

namespace FlatTop
{

    public readonly struct Coordinate
    {
        public int Row { get; }
        public int Col { get; }

        public Coordinate
        (
            int row,
            int col
        )
        {
            Row = row;
            Col = col;
        }

        // See DirectionLookup
        private static readonly Coordinate[] OddNeighbors =
        {
            new Coordinate(-1, 0),
            new Coordinate(0, +1),
            new Coordinate(+1, +1),
            new Coordinate(+1, 0),
            new Coordinate(+1, -1),
            new Coordinate(0, -1)
        };

        // See DirectionLookup
        private static readonly Coordinate[] OffsetEvenNeighbors =
        {
            new Coordinate(-1, 0),
            new Coordinate(-1, +1),
            new Coordinate(0, +1),
            new Coordinate(+1, 0),
            new Coordinate(0, -1),
            new Coordinate(-1, -1)
        };
        
        private static readonly Coordinate[][] DirectionLookup =
        {
            OffsetEvenNeighbors,
            OddNeighbors
        };

        public static Coordinate ExampleUsage
        (
            int oddOrEven,
            int neighborDirection
        )
        {
            return DirectionLookup[oddOrEven][neighborDirection];
        }
    }
}