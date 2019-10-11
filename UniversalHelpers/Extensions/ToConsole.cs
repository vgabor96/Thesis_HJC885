using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Interfaces;

namespace UniversalHelpers.Extensions
{
    public static class ToConsole
    {


        public static void Vectors_To_Console(IEnumerable<Vector2> vectors)
        {
            Console.WriteLine("Vectors:\n");
            foreach (Vector2 item in vectors)
            {
                Console.WriteLine(item.ToString() + "\n");
            }

        }

        public static void Bullets_To_Console(IEnumerable<IBullet> bullets)
        {
            Console.WriteLine("Bullets:\n");
            foreach (IBullet item in bullets)
            {
                Console.WriteLine(item.ToString() + "\n");
            }

        }

    }
}
