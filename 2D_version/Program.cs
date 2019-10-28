using _2D_version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Extensions;

namespace version_2D
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            // Console.WriteLine("Random Generált Vektorok koordinátái: \n");

            // // IEnumerable<Vector2> vs = RandomGenerator.Generate_Multiple_Random_Vector2(600, -45, 45);
            //List<Bullet> bs = new List<Bullet>();
            // for (int i = 0; i < 6; i++)
            // {
            //     Bullet b = new Bullet();
            //     b.GenerateRandomBullet();
            //     bs.Add(b);
            // }
    


                Robot robot = new Robot();

                List<Bullet> bs = new List<Bullet>();

                for (int i = 0; i < 10000; i++)
                {
                    Bullet b = new Bullet();
                    //TODO excpetion!!
                    
                    b.GenerateRandomBullet();
              
                    bs.Add(b);
                }

                UltimateVectorLogic logic = new UltimateVectorLogic();
            bs.Add(new Bullet(new Vector2(1, 1), 666, 666, 20, 11));
                int db = 0;
                string ishit;

                foreach (Bullet item in bs)
                {
                    if (logic.IsRobotHit_Console(robot, item))
                    {
                        ishit = "True";
                        db++;
                    }
                    else
                    {
                        ishit = "False";
                    }

                    Console.WriteLine($"ID:{item.ID}\t Size: {item.size} \t Vector: {item.Destination}\t\t WillHit?: {ishit}");
                }
                Console.WriteLine("Eltalálta:" + db);

                Console.ReadKey();
            }


           

        
    }
}
