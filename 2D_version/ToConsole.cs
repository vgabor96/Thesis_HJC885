using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public static class ToConsole
    {


        public static void Vectors_To_Console(IEnumerable<Vector2> vectors)
        {
            foreach (Vector2 item in vectors)
            {
                Console.WriteLine(item.ToString() + "\n");
                //Console.WriteLine("X: {0} \nY: {1} \nZ: {2}\n\n", item.X, item.Y, item.Z);
            }

        }

    }
}
