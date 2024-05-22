using System;

namespace FlatTop
{
    /// <summary>
    ///     We use "Flat top" style hexagon grids, relative to (0, 0, 0) World Space
    ///     .............  North
    ///     ----------
    ///     NorthWest      /TTT\     NorthEast
    ///     SouthWest      \___/     SouthEast
    ///     ----------
    ///     .............  South
    ///     Pointy top would be rotated 30 degrees
    /// </summary>
    public static partial class Neighbors
    {
        // TODO: I would like to think of a better name that clarifies what it is
        /// <summary>
        ///     Bitset enum for hexagonal directions
        /// </summary>
        [Flags]
        public enum HexagonalAxis : byte
        {
            None = 0,
            North = 1 << 0,
            NorthEast = 1 << 1,
            SouthEast = 1 << 2,
            South = 1 << 3,
            SouthWest = 1 << 4,
            NorthWest = 1 << 5,
            West = 1 << 6,
            East = 1 << 7,
            All = 63
        }

        public static Array FlagValues = Enum.GetValues(typeof(HexagonalAxis));

        public static AxisMasker ClearNeighborWithMask
        (
            ref HexagonalAxis neighbors,
            HexagonalAxis mask
        )
        {
            var masker = new AxisMasker
            {
                Mask = mask
            };
            return masker;
        }

        public static bool CheckNeighbor
        (
            HexagonalAxis neighbors,
            HexagonalAxis axis
        )
        {
            // The & operator will switch off all bits except for those specified in axis
            // If any of the bits in axis are not present in neighbors, this will return false
            return (neighbors & axis) == axis;
        }

        /// <summary>
        ///     This MUST only take in a single axis.
        /// </summary>
        /// <param name="neighbors"></param>
        /// <param name="axis"></param>
        public static void ClearNeighborAxis
        (
            ref HexagonalAxis neighbors,
            HexagonalAxis axis
        )
        {
            // Get the bit position of axis, shift binary 1 left, bitwise invert the result, this is the mask
            // bitwise & neighbors with this mask to turn off the bit of the selected axis
            neighbors = (HexagonalAxis) ((byte) neighbors & ~(0b1 << AxisToBitPosition(axis)));
        }

        public static void SetNeighborAxis
        (
            ref HexagonalAxis neighbors,
            HexagonalAxis axis
        )
        {
            neighbors |= axis;
        }

        /// <summary>
        ///     Switch based lookup used to map a HexagonalAxis to a single direction offset for coordinates.
        ///     When multiple direction bits are flipped, this will return North as default
        /// </summary>
        /// <param name="axis" type="HexagonalAxis">Bitset representing the direction</param>
        /// <returns></returns>
        public static byte AxisToOffsetIndex
        (
            HexagonalAxis axis
        )
        {
            return axis switch
            {
                HexagonalAxis.North => (byte) AxisIndex.North,
                HexagonalAxis.NorthEast => (byte) AxisIndex.NorthEast,
                HexagonalAxis.SouthEast => (byte) AxisIndex.SouthEast,
                HexagonalAxis.South => (byte) AxisIndex.South,
                HexagonalAxis.SouthWest => (byte) AxisIndex.SouthWest,
                HexagonalAxis.NorthWest => (byte) AxisIndex.NorthWest,
                _ => throw new ArgumentException("Input direction axis must be singular.")
            };
        }

        public static HexagonalAxis BitPositionToAxis
        (
            byte position
        )
        {
            var bit_position = (AxisBitPosition) position;
            return bit_position switch
            {
                AxisBitPosition.North => HexagonalAxis.North,
                AxisBitPosition.NorthEast => HexagonalAxis.NorthEast,
                AxisBitPosition.SouthEast => HexagonalAxis.SouthEast,
                AxisBitPosition.South => HexagonalAxis.South,
                AxisBitPosition.SouthWest => HexagonalAxis.SouthWest,
                AxisBitPosition.NorthWest => HexagonalAxis.NorthWest,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static byte AxisToBitPosition
        (
            HexagonalAxis axis
        )
        {
            return axis switch
            {
                HexagonalAxis.North => (byte) AxisBitPosition.North,
                HexagonalAxis.NorthEast => (byte) AxisBitPosition.NorthEast,
                HexagonalAxis.SouthEast => (byte) AxisBitPosition.SouthEast,
                HexagonalAxis.South => (byte) AxisBitPosition.South,
                HexagonalAxis.SouthWest => (byte) AxisBitPosition.SouthWest,
                HexagonalAxis.NorthWest => (byte) AxisBitPosition.NorthWest,
                _ => throw new ArgumentException("Input direction axis must be singular.")
            };
        }

        public static HexagonalAxis OppositeDirection
        (
            HexagonalAxis axis
        )
        {
            return axis switch
            {
                HexagonalAxis.North => HexagonalAxis.South,
                HexagonalAxis.NorthEast => HexagonalAxis.SouthWest,
                HexagonalAxis.SouthEast => HexagonalAxis.NorthWest,
                HexagonalAxis.South => HexagonalAxis.North,
                HexagonalAxis.SouthWest => HexagonalAxis.NorthEast,
                HexagonalAxis.NorthWest => HexagonalAxis.SouthEast,
                _ => throw new ArgumentException("Input direction axis must be singular.")
            };
        }

        /// <summary>
        ///     Given a neighbor bitset
        /// </summary>
        /// <param name="neighborFlag"></param>
        /// <returns></returns>
        public static uint Count
        (
            HexagonalAxis neighborFlag
        )
        {
            // TODO: a simple while-loop may be faster when counting up to 6 bits
            // This is a pop count, or hamming weight of the neighbor bitfield
            var i = (uint) neighborFlag;
            i -= (i >> 0b1) & 0b1010101010101010101010101010101; // add pairs of bits
            i = (i & 0b110011001100110011001100110011) + ((i >> 0b10) & 0b110011001100110011001100110011); // quads
            i = (i + (i >> 0b100)) & 0b1111000011110000111100001111; // groups of 8
            return (i * 0b1000000010000000100000001) >> 0b11000;
        }

        public struct AxisMasker
        {
            public HexagonalAxis Mask;

            public void AddMask
            (
                HexagonalAxis mask
            )
            {
                Mask |= mask;
            }

            public void TurnOffNeighbors
            (
                ref HexagonalAxis neighbors
            )
            {
                neighbors ^= Mask;
            }
        }

        /// <summary>
        ///     Used when converting a HexagonalAxis to a single direction offset for coordinates.
        ///     Starts with north and moves clockwise.
        /// </summary>
        public enum AxisIndex : byte
        {
            North = 3,
            NorthEast = 2,
            SouthEast = 1,
            South = 0,
            SouthWest = 5,
            NorthWest = 4
        }

        public enum AxisBitPosition : byte
        {
            North = 0,
            NorthEast = 1,
            SouthEast = 2,
            South = 3,
            SouthWest = 4,
            NorthWest = 5
        }
    }
}