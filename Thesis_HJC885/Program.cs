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
            Console.WriteLine("Random Generált Vektorok koordinátái: \n");

            IEnumerable<Vector3> vs = VectorGenerator.Generate_Multiple_Random_Vector3(6,40,45);



            ToConsole.Vectors_To_Console(vs);

            Console.ReadKey();
            
        }
    }
}
