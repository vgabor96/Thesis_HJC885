using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Thesis_HJC885
{
   public static class ToConsole
    {


        public static void Vectors_To_Console(IEnumerable<Vector3> vectors)
        {
            foreach (Vector3 item in vectors)
            {
                Console.WriteLine(item.ToString()+"\n");
                //Console.WriteLine("X: {0} \nY: {1} \nZ: {2}\n\n", item.X, item.Y, item.Z);
            }
          
        }

    }
}
