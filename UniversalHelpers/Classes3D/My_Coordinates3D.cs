using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using UniversalHelpers.Configurations;

namespace UniversalHelpers.Classes3D
{
   public class My_Coordinates3D
    {
       
            private int x;
            private int y;
        private int z;

            public int X { get => this.x; set => this.x = value; }
            public int Y { get => this.y; set => this.y = value; }
            public int Z { get => this.z; set => this.z = value; }

        public My_Coordinates3D(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }


            public static Vector3 GenerateRandomVector2()
            {
                float v1 = 0;
                float v2 = 0;
                float v3 = 0;

                v1 = (float)RandomGenerator3D.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

                //Vectors should moving => both of them cannot be 0.
                while (v1 == 0)
                {
                    v1 = (float)RandomGenerator3D.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

                }
                v2 = (float)RandomGenerator3D.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

                v3 = (float)RandomGenerator3D.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

            return new Vector3(v1, v2, v3);
            }

            public static Line LineFromTwoPoints(My_Coordinates3D first, My_Coordinates3D second)
            {
                Line line = new Line()
                {
                    X1 = first.X,
                    X2 = second.X,
                    Y1 = first.Y,
                    Y2 = second.Y,
                    
                };
                return line;

            }
   
    }

}
