using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Extensions;

namespace version_2D
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Random Generált Vektorok koordinátái: \n");

            // IEnumerable<Vector2> vs = RandomGenerator.Generate_Multiple_Random_Vector2(600, -45, 45);
           List<Bullet> bs = new List<Bullet>();
            for (int i = 0; i < 20; i++)
            {
                Bullet b = new Bullet();
                b.GenerateRandomBullet();
                bs.Add(b);
            }
          


            //ToConsole.Bullets_To_Console(bs);

            Map map = new Map(null,bs);
            Console.WriteLine(map);
            Console.ReadKey();

        }
    }
}
