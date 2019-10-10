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

           // IEnumerable<Vector2> vs = RandomGenerator.Generate_Multiple_Random_Vector2(600, -45, 45);

            IEnumerable<Bullet> bs = RandomGenerator.Generate_Multiple_Random_Bullet(20);


            ToConsole.Bullets_To_Console(bs);

            Console.ReadKey();

        }
    }
}
