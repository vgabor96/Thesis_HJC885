using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalHelpers.Configurations
{

    public static class Config
    {
    
        public const double DistanceRobotFromBuetStart = 200;
        public const int Default_Map_size_X = 600;
        public const int Default_Map_size_Y = 800;


        public const int Default_Vector_Count = 1;
        public const double Default_Vector_from = -6;
        public const double Default_Vector_to = 6;


        public const int Default_Bullet_Count = 1;

        public const int Default_Bullet_Location_x=150;
        public const int Default_Bullet_Location_y=150;

        public const double Default_Bullet_Speed = 10;
        public const double Max_Bullet_Speed = 10;
        public const double Min_Bullet_Speed = 1;

        public const double Default_Bullet_Size = 6;
        public const double Max_Bullet_Size = 10;
        public const double Min_Bullet_Size = 1;


        public const int Robot_Start_Location_X = 20;
        public const int Robot_Start_Location_Y = 20;
        public const double Robot_range = 1;

    }
}
