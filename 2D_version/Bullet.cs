using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    public class Bullet
    {
        static int id;
        double speed;
        double size;
        Vector2 destination;
        public Bullet(Vector2 destination, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size)
        {
            this.destination = destination;
            this.speed = speed;
            this.size = size;
            id++;
        }

        public override string ToString()
        {
            return $"ID: {id}\nDestination: {destination}\nSpeed: {speed}\nSize: {size}" ;
        }

    }
}
