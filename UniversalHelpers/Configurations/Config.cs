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
        public const int Default_Map_size_X = 800; // 800
        public const int Default_Map_size_Y = 600; // 600

        //VECTORS
        public const int Default_Vector_Count = 1; //1
        public const double Default_Vector_from = -6; //-6
        public const double Default_Vector_to = 6;  //+6

        //BULLET
        public const int Default_Bullet_Count = 10; //1

        public const int Default_Bullet_Location_x = Default_Map_size_X / 3;
        public const int Default_Bullet_Location_y = Default_Map_size_Y / 3;

        public const double Default_Bullet_Speed = 20; //10
        public const double Max_Bullet_Speed = 10;  //10
        public const double Min_Bullet_Speed = 1;  //1

        public const double Default_Bullet_Size_MAX = 15; //10
        public const double Default_Bullet_Size_MIN = 10; //10

        //BULLETHELPER
        public static bool Is_Line_Helper_On = true;

        // ROBOT
        public const int RobotID = 1; //1
        public const int Robot_Start_Location_X = Default_Map_size_X/2; //400
        public const int Robot_Start_Location_Y = Default_Map_size_Y/2; //300
        public const double Robot_size = 40; //40


        //RULES
        public const int Default_HitPoints = 10;

        //GAMESPEED
        public const double Game_Speed = 10; //10

    }
}
