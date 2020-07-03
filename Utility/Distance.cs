using Microsoft.Xna.Framework;
using System;

namespace Desolation.Utility
{
    public static partial class Util
    {
        public static float dist(float i, float j, float i2, float j2) => (float)Math.Sqrt(((i2 - i) * (i2 - i)) + ((j2 - j) * (j2 - j)));

        public static float dist(Vector2 a, Vector2 b) => dist(a.X, a.Y, b.X, b.Y);

        public static float dist(Vector2 v, float i, float j) => dist(v.X, v.Y, i, j);

        public static float dist(float i, float j, Vector2 v) => dist(v.X, v.Y, i, j);
    }
}