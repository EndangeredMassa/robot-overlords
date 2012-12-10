using System;

namespace RobotOverlords.Utility
{
    public static class Randomizer
    {
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static int NextInt(int start, int end)
        {
            return rand.Next(start, end);
        }

        public static double NextDouble(double start, double end)
        {
            return start + rand.NextDouble()*end;
        }
    }
}
