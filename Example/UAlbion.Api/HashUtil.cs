namespace UAlbion.Api
{
    public static class HashUtil
    {
        public static int RotateXor(int x, int y)
        {
            uint ux = unchecked((uint)x);
            uint uy = unchecked((uint)y);
            uint rotx = ((ux & 0xffff0000) >> 16) | ((ux & 0xffff) << 16);
            return unchecked((int)(rotx ^ uy));
        }

        public static int FNV1(int a, int b)
        {
            int hash = unchecked((int)2166136261);
            hash = unchecked((hash * 16777619) ^ a);
            hash = unchecked((hash * 16777619) ^ b);
            return hash;
        }
        public static int FNV1(int a, int b, int c)
        {
            int hash = unchecked((int)2166136261);
            hash = unchecked((hash * 16777619) ^ a);
            hash = unchecked((hash * 16777619) ^ b);
            hash = unchecked((hash * 16777619) ^ c);
            return hash;
        }
        public static int FNV1(int a, int b, int c, int d)
        {
            int hash = unchecked((int)2166136261);
            hash = unchecked((hash * 16777619) ^ a);
            hash = unchecked((hash * 16777619) ^ b);
            hash = unchecked((hash * 16777619) ^ c);
            hash = unchecked((hash * 16777619) ^ d);
            return hash;
        }
        public static int FNV1(int a, int b, int c, int d, int e)
        {
            int hash = unchecked((int)2166136261);
            hash = unchecked((hash * 16777619) ^ a);
            hash = unchecked((hash * 16777619) ^ b);
            hash = unchecked((hash * 16777619) ^ c);
            hash = unchecked((hash * 16777619) ^ d);
            hash = unchecked((hash * 16777619) ^ e);
            return hash;
        }
        public static int FNV1(int a, int b, int c, int d, int e, int f)
        {
            int hash = unchecked((int)2166136261);
            hash = unchecked((hash * 16777619) ^ a);
            hash = unchecked((hash * 16777619) ^ b);
            hash = unchecked((hash * 16777619) ^ c);
            hash = unchecked((hash * 16777619) ^ d);
            hash = unchecked((hash * 16777619) ^ e);
            hash = unchecked((hash * 16777619) ^ f);
            return hash;
        }
    }
}