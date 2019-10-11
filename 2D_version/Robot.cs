using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;

namespace version_2D
{
    public class Robot : IMapObject
    {


        My_Coordinates Actual_Location;
        double Range;
        //Vector2 Actualmovement;

        public Robot(int Default_Location_x = Config.Robot_Start_Location_X, int Default_Location_y = Config.Robot_Start_Location_Y,double Range = Config.Robot_range)
        {
            Actual_Location = new My_Coordinates(Default_Location_x, Default_Location_y);

            this.Range = Range;
        }

        public override string ToString()
        {
            return $"Position: [ {this.Actual_Location.X} , {this.Actual_Location.Y} ]\nRange: {Range}";
        }

        //public bool IsHit()
        //{
        //    try
        //    {
        //        if (Map.this.Current_Location[0]-this.Range)
        //        {

        //        }
        //    }
        //    catch (NullReferenceException)
        //    {

        //        //throw;
        //    }
        //    return false;
        //}

        public bool IsHarmful => false;

        public My_Coordinates Current_Location => this.Actual_Location;
    }
}
