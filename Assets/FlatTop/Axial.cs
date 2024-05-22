using System;
using static FlatTop.HashCode;
using static FlatTop.Neighbors;
using static FlatTop.Neighbors.HexagonalAxis;

namespace FlatTop
{
    public readonly struct AxialHexCoordinate :
        IEquatable<AxialHexCoordinate>,
        INeighbors<AxialHexCoordinate>,
        IDistance<AxialHexCoordinate>, IComparable<AxialHexCoordinate>, IHexCoordinate<AxialHexCoordinate>
    {
        public override int GetHashCode()
        {
            return Combine(Q, R);
        }

        public int Q { get; }
        public int R { get; }

        public AxialHexCoordinate
        (
            int q,
            int r
        )
        {
            Q = q;
            R = r;
        }

        // Arranged in fixed order, axis index is determined in Neighbors.AxisToInt
        private static readonly AxialHexCoordinate[] AxialNeighbors =
        {
            new AxialHexCoordinate(0, -1),
            new AxialHexCoordinate(+1, -1),
            new AxialHexCoordinate(1, 0),
            new AxialHexCoordinate(0, 1),
            new AxialHexCoordinate(-1, 1),
            new AxialHexCoordinate(-1, 0)
        };

        /// <summary>
        ///     Get the Axial Coordinate offset of a hexagon by direction.
        /// </summary>
        /// <param name="direction">Bitfield representing the offset to retrieve.</param>
        private static AxialHexCoordinate GetDirectionOffset
        (
            HexagonalAxis direction
        )
        {
            return AxialNeighbors[AxisToOffsetIndex(direction)];
        }

        /// <summary>
        ///     Get the Neighbor of the specified coordinate by adding the offset coordinate from the specified direction.
        /// </summary>
        /// <param name="coordIn">The AxialHexCoordinate whose neighbor we are finding.</param>
        /// <param name="direction">Direction is an integer starting North of a the hexagon moving clockwise.</param>
        /// <returns></returns>
        public AxialHexCoordinate GetNeighbor
        (
            HexagonalAxis direction
        )
        {
            return this + GetDirectionOffset(direction);
        }

        public CubeHexCoordinate ToCube()
        {
            return new CubeHexCoordinate
            (
                Q,
                R,
                -Q - R
            );
        }

        public OffsetHexCoordinate ToOffset()
        {
            var row = R + (Q - (Q & 1)) / 2;
            var col = Q;
            return new OffsetHexCoordinate(row, col);
        }

        public static AxialHexCoordinate operator +
        (
            AxialHexCoordinate coord1,
            AxialHexCoordinate coord2
        )
        {
            return new AxialHexCoordinate(coord1.Q + coord2.Q, coord1.R + coord2.R);
        }

        public static AxialHexCoordinate operator -
        (
            AxialHexCoordinate coord1,
            AxialHexCoordinate coord2
        )
        {
            return new AxialHexCoordinate(coord1.Q - coord2.Q, coord1.R - coord2.R);
        }

        public static implicit operator AxialHexCoordinate
        (
            CubeHexCoordinate coord
        )
        {
            return coord.ToAxial();
        }

        public static implicit operator AxialHexCoordinate
        (
            OffsetHexCoordinate coord
        )
        {
            return coord.ToAxial();
        }

        public static implicit operator AxialHexCoordinate
        (
            (int q, int r) coord
        )
        {
            return new AxialHexCoordinate(coord.q, coord.r);
        }

        public bool Equals
        (
            AxialHexCoordinate other
        )
        {
            return Q == other.Q && R == other.R;
        }

        // TODO: verify this is actually squared distance?
        // https://www.redblobgames.com/grids/hexagons/#distances
        public float GetEuclideanDistanceSquared
        (
            AxialHexCoordinate other
        )
        {
            var d_q = Q - other.Q;
            var d_r = R - other.R;
            return d_q * d_q + d_r * d_r + d_q * d_r;
        }

        public float GetDistance
        (
            AxialHexCoordinate other
        )
        {
            // TODO: Inline the calculations? Is that worthwhile? Am I smart enough?
            return ToCube().GetDistance(other);
        }

        public HexWithNeighbors<AxialHexCoordinate> GetNeighbors
        (
            HexagonalAxis neighbors = All
        )
        {
            return new HexWithNeighbors<AxialHexCoordinate>(this, neighbors);
        }

        public override bool Equals
        (
            object obj
        )
        {
            return obj is AxialHexCoordinate other && Equals(other);
        }

        public static bool operator ==
        (
            AxialHexCoordinate node1,
            AxialHexCoordinate node2
        )
        {
            return node1.Equals(node2);
        }

        public static bool operator !=
        (
            AxialHexCoordinate node1,
            AxialHexCoordinate node2
        )
        {
            return !node1.Equals(node2);
        }

        public override string ToString()
        {
            return $"(<Q, R> - <{Q}, {R}>)";
        }

        public AxialHexCoordinate ToAxial()
        {
            return this;
        }

        public int CompareTo
        (
            AxialHexCoordinate other
        )
        {
            var q_comparison = Q.CompareTo(other.Q);
            if (q_comparison != 0) return q_comparison;
            return R.CompareTo(other.R);
        }
    }
}