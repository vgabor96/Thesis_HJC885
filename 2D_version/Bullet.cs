using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;
using UniversalHelpers.Interfaces;

namespace version_2D
{
    public class Bullet : IMapObject, IBullet
    {
        static int idCounter = Config.RobotID+1;
        int id;
        public My_Coordinates current_Location;
        public My_Coordinates next_location;
        public Line line;
        public double size;
        Vector2 destination;

        public My_Coordinates Current_Location => this.current_Location;

        public bool IsHarmful => true;

        public double Speed { get => this.destination.Length();}
        public Vector2 Destination { get => destination; set => destination = value; }
        public int ID { get => id; set => id = value; }

        public Bullet()
        {

        }

        public Bullet(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y= Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size)
        {
            this.Ctor_helper(destination, start_Location_x, start_Location_y, size);
        }

        public void GenerateRandomBullet()
        {
  
            int current_LocationX = RandomGenerator.r.Next(0, Config.Default_Map_size_X);
            int current_LocationY = RandomGenerator.r.Next(0, Config.Default_Map_size_Y);
            Vector2 destination = RandomGenerator.Generate_Multiple_Random_Vector2().FirstOrDefault();
            int speed = RandomGenerator.r.Next(1,(int)Config.Default_Bullet_Speed*2);
            double size = RandomGenerator.r.Next(1, (int)Config.Default_Bullet_Size * 2);
            this.Ctor_helper(destination, current_LocationX, current_LocationY, size);
        }

        private void Ctor_helper(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, double size = Config.Default_Bullet_Size)
        {
            this.line = new Line();
            this.current_Location = new My_Coordinates(start_Location_x, start_Location_y);
            this.Destination = destination;
            this.size = size;
            this.ID = idCounter;
            idCounter++;
            NextLocationCalculator();

 

        }



        public void OneStep()
        {
            

            this.line = My_Coordinates.LineFromTwoPoints(this.current_Location, this.next_location);


            if (next_location.X< Config.Default_Map_size_X && next_location.X>=0 && next_location.Y < Config.Default_Map_size_Y && next_location.Y>=0)
            {
                this.current_Location = next_location;
            }
            else
            {
                this.current_Location.GenerateRandomCoordinate();
            }
            NextLocationCalculator();
        
        }
        private void NextLocationCalculator()
        {
            this.next_location = new My_Coordinates(this.current_Location.X + (int)destination.X, this.current_Location.Y + (int)destination.Y);
        }


        public override string ToString()
        {
            return $"ID: {ID}\nDestination: {Destination}\nSpeed: {Speed}\nSize: {size}" ;
        }


    }
}
