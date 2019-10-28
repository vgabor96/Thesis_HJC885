﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniversalHelpers.Classes3D;
using UniversalHelpers.Configurations;

namespace version_3D
{
    public class Robot3D
    {

        private int iD;
        My_Coordinates3D Actual_Location;
        public double Range;
        bool isHit => false;
        //Vector2 Actualmovement;

        public List<Rect> robotbody;
        public Robot3D(int Default_Location_x = Config.Robot_Start_Location_X, int Default_Location_y = Config.Robot_Start_Location_Y, int Default_Location_z = Config.Robot_Start_Location_Z, double range = Config.Robot_size)
        {

            this.ID = Config.RobotID;
            Actual_Location = new My_Coordinates3D(Default_Location_x, Default_Location_y,Default_Location_z);

            this.Range = range;
            BodyInitialization();
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

        public My_Coordinates3D Current_Location => this.Actual_Location;

        public int ID { get => iD; set => iD = value; }

        private void BodyInitialization()
        {

            this.robotbody = new List<Rect>();
            //Head
            this.robotbody.Add(new Rect(this.Actual_Location.X, this.Actual_Location.Y, (int)this.Range, (int)this.Range));
            //Body
            this.robotbody.Add(new Rect(this.Actual_Location.X - this.Range / 2, this.Actual_Location.Y + (int)this.Range, (int)this.Range * 2, (int)this.Range * 2));
            //LegLeft
            this.robotbody.Add(new Rect(this.Actual_Location.X - this.Range / 5, this.Actual_Location.Y + (int)this.Range * 3, (int)this.Range / 2, (int)this.Range));
            //LegRIght
            this.robotbody.Add(new Rect(this.Actual_Location.X + this.Range / 3 * 2, this.Actual_Location.Y + (int)this.Range * 3, (int)this.Range / 2, (int)this.Range));
        }
    }
}