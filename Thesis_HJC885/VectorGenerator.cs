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

        static Vector3 Generate_Random_Vector3(int from, int to)
        {
            return new Vector3((float)r.Next(from, to), (float)r.Next(from, to), (float)r.Next(from, to));
        }

    }
}
