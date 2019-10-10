using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public class Robot
    {


        double[] Current_Location = new double[2];
        double Range;
        //Vector2 Actualmovement;

        public Robot(double Default_Location_x = Config.Robot_Start_Location_X, double Default_Location_y = Config.Robot_Start_Location_Y,double Range = Config.Robot_range)
        {
            this.Current_Location[0] = Default_Location_x;
            this.Current_Location[1] = Default_Location_y;
            this.Range = Range;
        }

        public override string ToString()
        {
            return $"Position: [ {this.Current_Location[0]} , {this.Current_Location[1]} ]\nRange: {Range}";
        }


    }
}
