using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Thesis_HJC885
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Random Generált Vektor koordinátái: \n");

            Vector3 v = VectorGenerator.Generate_Random_Vector3(40,45);

            float[] vs = new float[] { v.X, v.Y, v.Z };

            Console.WriteLine("X: {0} \nY: {1} \nZ: {2}",vs[0],vs[1],vs[2]);

            Console.ReadKey();
            
        }
    }
}
