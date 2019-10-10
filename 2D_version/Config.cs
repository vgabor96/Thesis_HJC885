using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public static class Config
    {
        public const double DistanceRobotFromBuetStart = 200;
        public static readonly double[] Map_size = new double[] { 200, 200 };


        public const int Default_Vector_Count = 1;
        public const double Default_Vector_x = 45;
        public const double Default_Vector_y = 45;

        public const int Default_Bullet_Count = 1;
        public const double Default_Bullet_Speed = 2;
        public const double Default_Bullet_Size = 6;


        public static readonly double[] Robot_Start_Location = new double[] { 50, 50 };

    }
}
