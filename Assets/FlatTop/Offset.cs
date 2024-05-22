using System;
using static FlatTop.Neighbors;
using static FlatTop.Neighbors.HexagonalAxis;

namespace FlatTop
{
    /// <summary>
    ///     "Odd Q" flat top
    /// </summary>
    public readonly struct OffsetHexCoordinate :
        IHexCoordinate<OffsetHexCoordinate>,
        IEquatable<OffsetHexCoordinate>,
        IDistance<OffsetHexCoordinate>,
        INeighbors<OffsetHexCoordinate>
    {
        public int Row { get; }
        public int Col { get; }

        public OffsetHexCoordinate
        (
            int row,
            int col
        )
        {
            Row = row;
            Col = col;
        }

        // See DirectionLookup
        private static readonly OffsetHexCoordinate[] OffsetOddNeighbors =
        {
            new OffsetHexCoordinate(-1, 0),
            new OffsetHexCoordinate(0, +1),
            new OffsetHexCoordinate(+1, +1),
            new OffsetHexCoordinate(+1, 0),
            new OffsetHexCoordinate(+1, -1),
            new OffsetHexCoordinate(0, -1)
        };

        // See DirectionLookup
        private static readonly OffsetHexCoordinate[] OffsetEvenNeighbors =
        {
            new OffsetHexCoordinate(-1, 0),
            new OffsetHexCoordinate(-1, +1),
            new OffsetHexCoordinate(0, +1),
            new OffsetHexCoordinate(+1, 0),
            new OffsetHexCoordinate(0, -1),
            new OffsetHexCoordinate(-1, -1)
        };

        private static class OddColumn
        {
            // Add one to row
            public static readonly OffsetHexCoordinate North = (+1, +0);
            // Add one to row, Add one to column
            public static readonly OffsetHexCoordinate NorthEast = (+1, +1);
            // Add one to row, Subtract one from column
            public static readonly OffsetHexCoordinate NorthWest = (+1, -1);

            // Subtract one from column
            public static readonly OffsetHexCoordinate South = (-1, +0);
            // Add one to column
            public static readonly OffsetHexCoordinate SouthEast = (+0, +1);
            // Subtract one from column
            public static readonly OffsetHexCoordinate SouthWest = (+0, -1);
        }

        private static class EvenColumn
        {
            // Add one to row
            public static readonly OffsetHexCoordinate North = (+1, +0);
            // Add one to column
            public static readonly OffsetHexCoordinate NorthEast = (+0, +1);
            // Subtract one from column
            public static readonly OffsetHexCoordinate NorthWest = (+0, -1);

            // Subtract one from column
            public static readonly OffsetHexCoordinate South = (-1, +0);
            // Add one to column, Subtract one from row
            public static readonly OffsetHexCoordinate SouthEast = (-1, +1);
            // Subtract one to column, Subtract one from row
            public static readonly OffsetHexCoordinate SouthWest = (-1, -1);
        }


        // Grid is assumed to start with 0,0 in upper left
        // Rows increased downward and columns increased rightward
        // Going straight down/up is always +1/-1 to row
        // Going diagonally changes based on column parity (is it even)
        private static readonly OffsetHexCoordinate[][] DirectionLookup =
        {
            OffsetEvenNeighbors,
            OffsetOddNeighbors
        };

        private static OffsetHexCoordinate GetDirectionOffset
        (
            bool odd,
            HexagonalAxis direction
        )
        {
            if (odd)
            {
                // It's an odd column
                return direction switch
                {
                    North => OddColumn.North,
                    NorthEast => OddColumn.NorthEast,
                    SouthEast => OddColumn.SouthEast,
                    South => OddColumn.South,
                    SouthWest => OddColumn.SouthWest,
                    NorthWest => OddColumn.NorthWest,
                    _ => throw new ArgumentOutOfRangeException
                    (
                        nameof(direction),
                        direction,
                        null
                    )
                };
            }
            // It's an even column
            return direction switch
            {
                North => EvenColumn.North,
                NorthEast => EvenColumn.NorthEast,
                SouthEast => EvenColumn.SouthEast,
                South => EvenColumn.South,
                SouthWest => EvenColumn.SouthWest,
                NorthWest => EvenColumn.NorthWest,
                _ => throw new ArgumentOutOfRangeException
                (
                    nameof(direction),
                    direction,
                    null
                )
            };
        }

        /// <summary>
        ///     Get the Neighbor of the specified coordinate by adding the offset coordinate from the specified direction.
        /// </summary>
        /// <param name="direction">Direction is an integer starting North of a the hexagon moving clockwise.</param>
        /// <returns></returns>
        public OffsetHexCoordinate GetNeighbor
        (
            HexagonalAxis direction
        )
        {
            return this + GetDirectionOffset((Col & 1) == 1, direction);
        }

        public HexWithNeighbors<OffsetHexCoordinate> GetNeighbors
        (
            HexagonalAxis neighbors = All
        )
        {
            return new HexWithNeighbors<OffsetHexCoordinate>(this, neighbors);
        }

        public float GetDistance
        (
            OffsetHexCoordinate other
        )
        {
            return ToCube().GetDistance(other.ToCube());
        }

        public static OffsetHexCoordinate operator +
        (
            OffsetHexCoordinate coord1,
            OffsetHexCoordinate coord2
        )
        {
            return new OffsetHexCoordinate(coord1.Row + coord2.Row, coord1.Col + coord2.Col);
        }

        public static OffsetHexCoordinate operator -
        (
            OffsetHexCoordinate coord1,
            OffsetHexCoordinate coord2
        )
        {
            return new OffsetHexCoordinate(coord1.Row - coord2.Row, coord1.Col - coord2.Col);
        }

        public static implicit operator OffsetHexCoordinate
        (
            CubeHexCoordinate coord
        )
        {
            return coord.ToOffset();
        }

        public static implicit operator OffsetHexCoordinate
        (
            AxialHexCoordinate coord
        )
        {
            return coord.ToOffset();
        }

        public static implicit operator OffsetHexCoordinate
        (
            (int Row, int Col) coord
        )
        {
            return new OffsetHexCoordinate(coord.Row, coord.Col);
        }


        public bool Equals
        (
            OffsetHexCoordinate other
        )
        {
            return Col == other.Col && Row == other.Row;
        }

        public static bool operator ==
        (
            OffsetHexCoordinate node1,
            OffsetHexCoordinate node2
        )
        {
            return node1.Equals(node2);
        }

        public static bool operator !=
        (
            OffsetHexCoordinate node1,
            OffsetHexCoordinate node2
        )
        {
            return !node1.Equals(node2);
        }

        public override string ToString()
        {
            return $"(<Row, Col> - <{Row}, {Col}>)";
        }

        public override bool Equals
        (
            object obj
        )
        {
            return obj is OffsetHexCoordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Col >= Row ? Col * Col + Col + Row : Col + Row * Row;
        }

        public OffsetHexCoordinate ToOffset()
        {
            return this;
        }

        public CubeHexCoordinate ToCube()
        {
            var q = Col;
            var r = Row - (Col - (Col & 1)) / 2;
            return new CubeHexCoordinate
            (
                q,
                r,
                -q - r
            );
        }

        public AxialHexCoordinate ToAxial()
        {
            var q = Col;
            var r = Row - (Col - (Col & 1)) / 2;
            return new AxialHexCoordinate
            (
                q,
                r
            );
        }
    }
}