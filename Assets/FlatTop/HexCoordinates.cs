using static FlatTop.Neighbors;

namespace FlatTop
{

    public interface IHexCoordinate<THex> : IConvertsToCube, IConvertsToAxial, IConvertsToOffset, INeighbors<THex>
        where THex : unmanaged, IHexCoordinate<THex>
    {

    }

    public interface IDistance<in T> where T : unmanaged
    {
        public float GetDistance
        (
            T other
        );
    }

    public interface INeighbors<T> where T : unmanaged, IHexCoordinate<T>
    {
        public HexWithNeighbors<T> GetNeighbors
        (
            HexagonalAxis flag = HexagonalAxis.All
        );
        public T GetNeighbor
        (
            HexagonalAxis direction
        );
    }

    public interface ICoordinateWithNeighbors<T> : INeighbors<T> where T : unmanaged, IHexCoordinate<T>
    {
        public T Node { get; }
    }

    public interface IConvertsToCube
    {
        public CubeHexCoordinate ToCube();
    }

    public interface IConvertsToOffset
    {
        public OffsetHexCoordinate ToOffset();
    }

    public interface IConvertsToAxial
    {
        public AxialHexCoordinate ToAxial();
    }

    public static class HexCoordinates
    {
        public static AxialHexCoordinate Zero => new AxialHexCoordinate(0, 0);
        public static AxialHexCoordinate UndefinedLocation => new AxialHexCoordinate(int.MaxValue, int.MaxValue);
    }

}