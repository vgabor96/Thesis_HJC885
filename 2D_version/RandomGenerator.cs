using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public static class RandomGenerator
    {
        static readonly Random r = new Random();

        public static IEnumerable<Vector2> Generate_Multiple_Random_Vector2(int count=Config.Default_Vector_Count, int from=45, int to=45)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector2((float)r.Next(from, to), (float)r.Next(from, to));
            }

        }

        public static IEnumerable<Bullet> Generate_Multiple_Random_Bullet(int count = Config.Default_Bullet_Count)
        {
            Vector2[] vectors = Generate_Multiple_Random_Vector2(count).ToArray();
            for (int i = 0; i < count; i++)
            {
                yield return new Bullet(vectors[i]);
            }

        }

    }
}
