using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Configurations;

namespace UniversalHelpers.Classes3D
{
    public static class RandomGenerator3D
    { 

            public static readonly Random r = new Random();

            public static IEnumerable<Vector3> Generate_Multiple_Random_Vector2(int count = Config.Default_Vector_Count, double from = Config.Default_Vector_from, double to = Config.Default_Vector_to)
            {
                float v1 = 0;
                float v2 = 0;
                float v3 = 0;
                for (int i = 0; i < count; i++)
                {



                    v1 = (float)r.Next(Convert.ToInt32(from), Convert.ToInt32(to));



                    v2 = (float)r.Next(Convert.ToInt32(from), Convert.ToInt32(to));

                    v3 = (float)r.Next(Convert.ToInt32(from), Convert.ToInt32(to));

                    yield return new Vector3(v1, v2,v3);
                }

            }
        }
}
