using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;
using UniversalHelpers.Extensions;

namespace version_3D
{
    public class Map
    {
        public My_Coordinates size;
        public Robot robot;
        public List<Bullet> bullets;
        public double[,] mapObjects;

        public Map(Robot robot = null, IEnumerable<Bullet> bullets = null)
        {
            this.robot = robot;
            this.bullets = new List<Bullet>();

            this.size = new My_Coordinates(Config.Default_Map_size_X, Config.Default_Map_size_Y);
            mapObjects = new double[this.size.X, this.size.Y];

            My_Coordinates current_location;

            if (robot != null)
            {
                current_location = robot.Current_Location;
            }
            else
            {
                this.robot = new Robot();
                current_location = new My_Coordinates(this.robot.Current_Location.X, this.robot.Current_Location.Y);

            }

            mapObjects.Set_IMapObjectElement(current_location.X, current_location.Y, this.robot.ID);

            if (bullets != null)
            {
                foreach (Bullet item in bullets)
                {
                    this.bullets.Add(item);
                    mapObjects.Set_IMapObjectElement(item.Current_Location.X, item.Current_Location.Y, item.ID);
                }
            }


        }

        public bool OneTick()
        {
            mapObjects.Set_IMapObjectElement(robot.Current_Location, this.robot.ID);
            foreach (Bullet item in this.bullets)
            {
                mapObjects.Set_IMapObjectElement(item.Current_Location, 0);
                item.OneStep();
                mapObjects.Set_IMapObjectElement(item.Current_Location, item.ID);
            }
            return RobotIshit();
        }

        public bool RobotIshit()
        {
            Robot robot = this.robot;

            double result = 1;
            for (int i = 0; i < this.mapObjects.GetLength(0); i++)
            {
                for (int j = 0; j < this.mapObjects.GetLength(1); j++)
                {
                    result = Math.Sqrt(Math.Pow(robot.Current_Location.X - i, 2) + Math.Pow(robot.Current_Location.Y - j, 2));
                    return result == 0;
                }
            }
            return false;

        }



        public override string ToString()
        {
            return mapObjects.MapToString();
        }


    }
}
