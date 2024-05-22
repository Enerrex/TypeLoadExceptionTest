using System;
using static FlatTop.Neighbors;
using static FlatTop.Neighbors.HexagonalAxis;

namespace FlatTop
{
    public readonly struct CubeHexCoordinate :
        IHexCoordinate<CubeHexCoordinate>,
        IEquatable<CubeHexCoordinate>,
        INeighbors<CubeHexCoordinate>,
        IDistance<CubeHexCoordinate>
    {
        public int Q { get; }
        public int R { get; }
        public int S { get; }

        public CubeHexCoordinate
        (
            int q,
            int r,
            int s
        )
        {
            Q = q;
            R = r;
            S = s;
        }

        // Arranged in fixed order, axis index is determined in Neighbors.AxisToInt
        private static readonly CubeHexCoordinate[] CubeNeighbors =
        {
            new CubeHexCoordinate
            (
                0,
                -1,
                1
            ),
            new CubeHexCoordinate
            (
                +1,
                -1,
                0
            ),
            new CubeHexCoordinate
            (
                1,
                0,
                -1
            ),
            new CubeHexCoordinate
            (
                0,
                1,
                -1
            ),
            new CubeHexCoordinate
            (
                -1,
                1,
                0
            ),
            new CubeHexCoordinate
            (
                -1,
                0,
                1
            )
        };

        /// <summary>
        ///     Get the Cube Coordinate offset of a hexagon by direction.
        /// </summary>
        /// <param name="direction">Direction is an integer starting North of a the hexagon moving clockwise.</param>
        private static CubeHexCoordinate GetDirectionOffset
        (
            HexagonalAxis direction
        )
        {
            return CubeNeighbors[AxisToOffsetIndex(direction)];
        }

        /// <summary>
        ///     Get the Neighbor of the specified coordinate by adding the offset coordinate from the specified direction.
        /// </summary>
        /// <param name="direction">Direction is an integer starting North of a the hexagon moving clockwise.</param>
        /// <returns></returns>
        public CubeHexCoordinate GetNeighbor
        (
            HexagonalAxis direction
        )
        {
            return this + GetDirectionOffset(direction);
        }

        public HexWithNeighbors<CubeHexCoordinate> GetNeighbors
        (
            HexagonalAxis neighbors = All
        )
        {
            return new HexWithNeighbors<CubeHexCoordinate>(this, neighbors);
        }

        public AxialHexCoordinate ToAxial()
        {
            return new AxialHexCoordinate(Q, R);
        }

        public OffsetHexCoordinate ToOffset()
        {
            var row = R + (Q - (Q & 1)) / 2;
            var col = Q;
            return new OffsetHexCoordinate(row, col);
        }

        public CubeHexCoordinate RotateLeft
        (
            CubeHexCoordinate center
        )
        {
            var rotation_vector = this - center;
            rotation_vector = new CubeHexCoordinate
            (
                -rotation_vector.R,
                -rotation_vector.S,
                -rotation_vector.Q
            );
            return rotation_vector + center;
        }

        public static CubeHexCoordinate operator +
        (
            CubeHexCoordinate coord1,
            CubeHexCoordinate coord2
        )
        {
            return new CubeHexCoordinate
            (
                coord1.Q + coord2.Q,
                coord1.R + coord2.R,
                coord1.S + coord2.S
            );
        }

        public static CubeHexCoordinate operator -
        (
            CubeHexCoordinate coord1,
            CubeHexCoordinate coord2
        )
        {
            return new CubeHexCoordinate
            (
                coord1.Q - coord2.Q,
                coord1.R - coord2.R,
                coord1.S - coord2.S
            );
        }

        public static implicit operator CubeHexCoordinate
        (
            AxialHexCoordinate coord
        )
        {
            return coord.ToCube();
        }

        public static implicit operator CubeHexCoordinate
        (
            OffsetHexCoordinate coord
        )
        {
            return coord.ToCube();
        }

        public static implicit operator CubeHexCoordinate
        (
            (int q, int r, int s) coord
        )
        {
            return new CubeHexCoordinate
            (
                coord.q,
                coord.r,
                coord.s
            );
        }

        public override string ToString()
        {
            return $"(<Q, R, S> - <{Q}, {R}, {S}>)";
        }

        public float GetDistance
        (
            CubeHexCoordinate coordinate
        )
        {
            var direction_vector = this - coordinate.ToCube();
            return Math.Max
            (
                Math.Max
                (
                    Math.Abs(direction_vector.Q),
                    Math.Abs(direction_vector.R)
                ),
                Math.Abs(direction_vector.S)
            );
        }

        public CubeHexCoordinate ToCube()
        {
            return this;
        }

        public bool Equals
        (
            CubeHexCoordinate other
        )
        {
            return Q == other.Q && R == other.R && S == other.S;
        }


        public override bool Equals
        (
            object obj
        )
        {
            return obj is CubeHexCoordinate other && Equals(other);
        }

        public static bool operator ==
        (
            CubeHexCoordinate node1,
            CubeHexCoordinate node2
        )
        {
            return node1.Equals(node2);
        }

        public static bool operator !=
        (
            CubeHexCoordinate node1,
            CubeHexCoordinate node2
        )
        {
            return !node1.Equals(node2);
        }

        /// <summary>
        ///     This assumes all positive coordinates, that's fine. We don't need negatives as we control (0,0,0
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var q = Q >= 0 ? 2 * Q : -2 * Q - 1;
            var r = R >= 0 ? 2 * R : -2 * R - 1;
            var s = S >= 0 ? 2 * S : -2 * S - 1;
            var max = Math.Max(Math.Max(q, R), S);
            var hash = max ^ (3 + 2 * max * s + s);
            if (max == s) hash += Math.Max(q, r) ^ 2;
            if (r >= q)
                hash += q + r;
            else
                hash += r;
            return hash;
        }
    }
}