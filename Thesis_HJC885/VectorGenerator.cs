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
        static readonly Random r = new Random();

        public static IEnumerable<Vector3> Generate_Multiple_Random_Vector3(int count=1, int from=45, int to=45)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector3((float)r.Next(from, to), (float)r.Next(from, to), (float)r.Next(from, to));
            }
          
        }

    }


}
