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
        static void Main()
        {
            Console.WriteLine("Random Generált Vektorok koordinátái: \n");

            IEnumerable<Vector3> vs = VectorGenerator.Generate_Multiple_Random_Vector3(6000,-45,45);



            ToConsole.Vectors_To_Console(vs);

            Console.ReadKey();
            
        }
    }
}
