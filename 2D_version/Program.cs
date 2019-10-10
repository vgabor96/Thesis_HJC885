using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Random Generált Vektorok koordinátái: \n");

            IEnumerable<Vector2> vs = VectorGenerator.Generate_Multiple_Random_Vector3(600, -45, 45);



            ToConsole.Vectors_To_Console(vs);

            Console.ReadKey();

        }
    }
}
