using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;
using UniversalHelpers.Enumerators;
using UniversalHelpers.Extensions;

namespace version_2D
{
    public class Map
    {
        My_Coordinates size;
        Robot robot;
        List<Bullet> bullets;
        double[,] mapObjects;

        public Map(Robot robot = null,IEnumerable<Bullet> bullets = null)
        {
            this.robot = robot;
            this.bullets = new List<Bullet>();
            
            this.size = new My_Coordinates(Config.Default_Map_size_X, Config.Default_Map_size_Y);
            mapObjects = new double[this.size.X,this.size.Y];
            
            My_Coordinates current_location;
            
            if (robot != null)
            {
                current_location = robot.Current_Location;    
            }
            else
            {
                this.robot = new Robot();
                current_location = new My_Coordinates(this.robot.Current_Location.X,this.robot.Current_Location.Y);
             
            }

            mapObjects.Set_IMapObjectElement(current_location.X, current_location.Y, MapObjectType.Robot);

            foreach (Bullet item in bullets)
            {
                this.bullets.Add(item);
                mapObjects.Set_IMapObjectElement(item.Current_Location.X, item.Current_Location.Y, MapObjectType.Bullet);
            }

            mapObjects.DoesContainsThisCoordinate(new My_Coordinates(0, 1));
         }

        public void OneTick()
        {
            mapObjects.Set_IMapObjectElement(robot.Current_Location, MapObjectType.Robot);
            foreach (Bullet item in this.bullets)
            {
                mapObjects.Set_IMapObjectElement(item.Current_Location, 0);
                item.OneStep();
                mapObjects.Set_IMapObjectElement(item.Current_Location, MapObjectType.Bullet);
            }
        }
        public override string ToString()
        {
            return mapObjects.MapToString();
        }


    }
}
