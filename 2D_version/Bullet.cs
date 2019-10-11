using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;
using UniversalHelpers.Interfaces;

namespace version_2D
{
    public class Bullet : IMapObject, IBullet
    {
        static int idCounter = 0;
        int id;
        My_Coordinates current_Location;
        double speed;
        double size;
        Vector2 destination;
        public Bullet()
        {

        }

        public Bullet(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y= Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size)
        {
            this.Ctor_helper(destination, start_Location_x, start_Location_y, speed, size);
        }

        public void GenerateRandomBullet()
        {
  
            int current_LocationX = RandomGenerator.r.Next(0, Config.Default_Map_size_X);
            int current_LocationY = RandomGenerator.r.Next(0, Config.Default_Map_size_Y);
            Vector2 destination = RandomGenerator.Generate_Multiple_Random_Vector2().FirstOrDefault();
            int speed = RandomGenerator.r.Next(1,(int)Config.Default_Bullet_Speed*2);
            double size = RandomGenerator.r.Next(1, (int)Config.Default_Bullet_Size * 2);
            this.Ctor_helper(destination, current_LocationX, current_LocationY, speed, size);
        }

        private void Ctor_helper(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size)
        {
            this.current_Location = new My_Coordinates(start_Location_x, start_Location_y);
            this.destination = destination;
            this.speed = speed;
            this.size = size;
            this.id = idCounter;
            idCounter++;
        }

        public My_Coordinates Current_Location => this.current_Location;

        public bool IsHarmful => true;

        public override string ToString()
        {
            return $"ID: {id}\nDestination: {destination}\nSpeed: {speed}\nSize: {size}" ;
        }
    }
}
