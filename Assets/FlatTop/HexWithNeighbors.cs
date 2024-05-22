using System;
using static FlatTop.Neighbors;

namespace FlatTop
{
    public struct HexWithNeighbors<THex> where THex : unmanaged, INeighbors<THex>, IHexCoordinate<THex>
    {
        public HexagonalAxis Neighbors;
        public THex Coordinate;

        public HexWithNeighbors
        (
            THex coordinate,
            HexagonalAxis neighbors = HexagonalAxis.All
        )
        {
            Neighbors = neighbors;
            Coordinate = coordinate;
        }

        public THex this
        [
            HexagonalAxis index
        ]
        {
            get
            {
                return index switch
                {
                    HexagonalAxis.North => Coordinate.GetNeighbor(HexagonalAxis.North),
                    HexagonalAxis.NorthEast => Coordinate.GetNeighbor(HexagonalAxis.NorthEast),
                    HexagonalAxis.SouthEast => Coordinate.GetNeighbor(HexagonalAxis.SouthEast),
                    HexagonalAxis.South => Coordinate.GetNeighbor(HexagonalAxis.South),
                    HexagonalAxis.SouthWest => Coordinate.GetNeighbor(HexagonalAxis.SouthWest),
                    HexagonalAxis.NorthWest => Coordinate.GetNeighbor(HexagonalAxis.NorthWest),
                    _ => throw new ArgumentException("Hexagons only have 6 sides dummy.")
                };
            }
        }

        public readonly HexWithNeighborsIterator<THex> GetIterator()
        {
            return new HexWithNeighborsIterator<THex>(this);
        }

        public THex North => Coordinate.GetNeighbor(HexagonalAxis.North);
        public THex NorthEast => Coordinate.GetNeighbor(HexagonalAxis.NorthEast);
        public THex SouthEast => Coordinate.GetNeighbor(HexagonalAxis.SouthEast);
        public THex South => Coordinate.GetNeighbor(HexagonalAxis.South);
        public THex SouthWest => Coordinate.GetNeighbor(HexagonalAxis.SouthWest);
        public THex NorthWest => Coordinate.GetNeighbor(HexagonalAxis.NorthWest);
    }
}