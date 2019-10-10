using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public class Bullet : IMapObject
    {
        static int id;
        double[] current_Location = new double[2];
        double speed;
        double size;
        Vector2 destination;
        public Bullet(Vector2 destination, double start_Location_x = Config.Default_Bullet_Location_x, double start_Location_y= Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size)
        {

            this.current_Location[0] = start_Location_x;
            this.current_Location[1] = start_Location_y;
            this.destination = destination;
            this.speed = speed;
            this.size = size;
            id++;
        }

        public double[] Current_Location => this.current_Location;

        public bool IsHarmful => true;

        public override string ToString()
        {
            return $"ID: {id}\nDestination: {destination}\nSpeed: {speed}\nSize: {size}" ;
        }

    }
}
