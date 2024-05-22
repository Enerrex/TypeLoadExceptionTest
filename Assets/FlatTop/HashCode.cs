namespace FlatTop
{
    public static class HashCode
    {
        public static int Combine
        (
            object h1,
            object h2
        )
        {
            return ((h1.GetHashCode() << 5) + h1.GetHashCode()) ^ (int) h2;
        }

        public static int Combine
        (
            int h1,
            int h2
        )
        {
            return ((h1.GetHashCode() << 5) + h1.GetHashCode()) ^ h2;
        }
    }
}