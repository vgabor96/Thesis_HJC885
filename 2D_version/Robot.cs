using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public class Robot : IMapObject
    {


        double[] Actual_Location = new double[2];
        double Range;
        //Vector2 Actualmovement;

        public Robot(double Default_Location_x = Config.Robot_Start_Location_X, double Default_Location_y = Config.Robot_Start_Location_Y,double Range = Config.Robot_range)
        {
            this.Actual_Location[0] = Default_Location_x;
            this.Actual_Location[1] = Default_Location_y;
            this.Range = Range;
        }

        public override string ToString()
        {
            return $"Position: [ {this.Actual_Location[0]} , {this.Actual_Location[1]} ]\nRange: {Range}";
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

        public double[] Current_Location => this.Actual_Location;
    }
}
