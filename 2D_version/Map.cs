using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Extensions;

namespace _2D_version
{
    public class Map
    {
        double[] Map_Size;
        double[,] mapObjects;

        public Map(double map_size_x = Config.Default_Map_size_X, double map_size_y = Config.Default_Map_size_Y,Robot robot = null,IEnumerable<Bullet> bullets = null)
        {
            this.Map_Size[0] = map_size_x;
            this.Map_Size[1] = map_size_y;

            mapObjects = new double[(int)Map_Size[0],(int)Map_Size[1]];
            if (robot != null)
            {
                double[] current_location = robot.Current_Location;
                mapObjects[(int)current_location[0], (int)current_location[1]] = (double)Config.MapObjectType.Robot;

                foreach (Bullet item in bullets)
                {
                    current_location = item.Current_Location;
                    mapObjects[(int)current_location[0], (int)current_location[1]] = (double)Config.MapObjectType.Bullet;
                }
            }

            mapObjects.DoesContainsThisCoordinate(new My_Coordinates(0, 1));


         }


    }
}
