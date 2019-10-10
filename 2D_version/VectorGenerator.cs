using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public static class VectorGenerator
    {
        static readonly Random r = new Random();

        public static IEnumerable<Vector2> Generate_Multiple_Random_Vector3(int count, int from, int to)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector2((float)r.Next(from, to), (float)r.Next(from, to));
            }

        }

    }
}
