using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalHelpers.Configurations
{

    public static class Config
    {
        //MAP
        public const double DistanceRobotFromBuetStart = 200;
        public const int Default_Map_size_X = 800;
        public const int Default_Map_size_Y = 600;

        //VECTORS
        public const int Default_Vector_Count = 1;
        public const double Default_Vector_from = -6;
        public const double Default_Vector_to = 6;

        //BULLET
        public const int Default_Bullet_Count = 1;

        public const int Default_Bullet_Location_x=150;
        public const int Default_Bullet_Location_y=150;

        public const double Default_Bullet_Speed = 10;
        public const double Max_Bullet_Speed = 10;
        public const double Min_Bullet_Speed = 1;

        public const double Default_Bullet_Size = 20;


        // ROBOT
        public const int RobotID = 1;
        public const int Robot_Start_Location_X = 400;
        public const int Robot_Start_Location_Y = 300;
        public const double Robot_range = 40;

    }
}
