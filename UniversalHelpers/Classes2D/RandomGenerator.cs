using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Configurations;

namespace UniversalHelpers.Classes2D
{
    public static class RandomGenerator
    {
       public static readonly Random r = new Random();

        public static IEnumerable<Vector2> Generate_Multiple_Random_Vector2(int count = Config.Default_Vector_Count, double from = Config.Default_Vector_from, double to = Config.Default_Vector_to)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector2((float)r.Next(Convert.ToInt32(from), Convert.ToInt32(to)), (float)r.Next(Convert.ToInt32(from), Convert.ToInt32(to)));
            }

        }

        //public static IEnumerable<Bullet> Generate_Multiple_Random_Bullet(int count = Config.Default_Bullet_Count, bool randomsize_and_speed = false, bool randomdlocation = false)
        //{
        //    Vector2[] vectors = Generate_Multiple_Random_Vector2(count).ToArray();


        //    if (randomsize_and_speed)
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            int[] values = new int[] { r.Next((int)Math.Round(Config.Min_Bullet_Speed), (int)Math.Round(Config.Max_Bullet_Speed)), r.Next((int)Math.Round(Config.Min_Bullet_Size), (int)Math.Round(Config.Max_Bullet_Size)) };
        //            yield return new Bullet(vectors[i], values[0], values[1], values[2], values[3]);
        //        }
        //    }

        //    else
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            yield return new Bullet(vectors[i]);
        //        }
        //    }



        //}

        //public static IEnumerable<Bullet> Generate_Multiple_Random_Bullet(int count = Config.Default_Bullet_Count, bool randomsize_and_speed = false)
        //{
        //    Vector2[] vectors = Generate_Multiple_Random_Vector2(count).ToArray();


        //    if (randomsize_and_speed)
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            int[] values = new int[] { r.Next((int)Math.Round(Config.Min_Bullet_Speed), (int)Math.Round(Config.Max_Bullet_Speed)), r.Next((int)Math.Round(Config.Min_Bullet_Size), (int)Math.Round(Config.Max_Bullet_Size)) };
        //            yield return new Bullet(vectors[i], values[0], values[1], values[2], values[3]);
        //        }
        //    }

        //    else
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            yield return new Bullet(vectors[i]);
        //        }
        //    }



        //}


    }
}
