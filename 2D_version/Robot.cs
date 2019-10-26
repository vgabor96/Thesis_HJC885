using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;

namespace version_2D
{
    public class Robot : IMapObject
    {

        private int iD;
        My_Coordinates Actual_Location;
        public double Range;
        bool isHit => false;
        //Vector2 Actualmovement;

        public Rect robotbody;
        public Robot(int Default_Location_x = Config.Robot_Start_Location_X, int Default_Location_y = Config.Robot_Start_Location_Y,double range = Config.Robot_size)
        {
            this.robotbody = new Rect(Default_Location_x, Default_Location_y, (int)range, (int)range);
            this.ID = Config.RobotID;
            Actual_Location = new My_Coordinates(Default_Location_x, Default_Location_y);

            this.Range = range;
        }

        public override string ToString()
        {
            return $"Position: [ {this.Actual_Location.X} , {this.Actual_Location.Y} ]\nRange: {Range}";
        }

        //public bool IsHit()
        //{
        //    try
        //    {
        //        if (Map.this.Current_Location[0] - this.Range)
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

        public int ID { get => iD; set => iD = value; }
    }
}
