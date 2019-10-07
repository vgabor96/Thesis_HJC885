using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Thesis_HJC885
{
    public static class VectorGenerator
    {
        static Random r = new Random();

       public static Vector3 Generate_Random_Vector3(int from, int to)
        {
            return new Vector3((float)r.Next(from, to), (float)r.Next(from, to), (float)r.Next(from, to));
        }

        public static IEnumerable<Vector3> Generate_MultipleRandom_Vector3(int count, int from, int to)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector3((float)r.Next(from, to), (float)r.Next(from, to), (float)r.Next(from, to));
            }
          
        }

    }


}
