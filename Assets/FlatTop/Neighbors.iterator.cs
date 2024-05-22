namespace FlatTop
{
    public static partial class Neighbors
    {
        public struct HexagonalAxisIterator
        {
            private byte _neighborIndex;

            public HexagonalAxis Current => BitPositionToAxis((byte) (_neighborIndex - 1));

            public bool MoveNext()
            {
                _neighborIndex++;
                return _neighborIndex - 1 < 6;
            }
        }
    }
}