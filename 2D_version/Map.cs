using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public class Map
    {
        double[] Map_Size;
        IMapObject[,] mapObjects;

        public Map(double map_size_x = Config.Default_Map_size_X, double map_size_y = Config.Default_Map_size_Y,Robot robot = null,IEnumerable<Bullet> bullets = null)
        {
            mapObjects = new IMapObject[(int)Map_Size[0],(int)Map_Size[1]];
            if (robot != null)
            {
                double[] current_location = robot.Current_Location;
                mapObjects[(int)current_location[0], (int)current_location[1]] = robot;

                foreach (Bullet item in bullets)
                {
                    current_location = item.Current_Location;
                    mapObjects[(int)current_location[0], (int)current_location[1]] = item;
                }
            }
            
            this.Map_Size[0] = map_size_x;
            this.Map_Size[1] = map_size_y;
         }


    }
}
