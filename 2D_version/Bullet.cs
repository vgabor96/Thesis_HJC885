using System;
using System.Collections.Generic;
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
        public  Line[] lines;
        public double size;
        Vector2 destination;

        public My_Coordinates Current_Location => this.current_Location;

        public bool IsHarmful { get; set; }

        public double Speed { get => this.destination.Length();}
        public Vector2 Destination { get => destination; set => destination = value; }
        public int ID { get => id; set => id = value; }

        public Bullet()
        {

        }

        public Bullet(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y= Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size_MAX)
        {
            this.Ctor_helper(destination, start_Location_x, start_Location_y, size);
        }

        public void GenerateRandomBullet()
        {

            int current_LocationX = Config.Default_Bullet_Location_x;//RandomGenerator.r.Next(0, Config.Default_Map_size_X);
            int current_LocationY = Config.Default_Bullet_Location_y;//RandomGenerator.r.Next(0, Config.Default_Map_size_Y);
            Vector2 destination = My_Coordinates.GenerateRandomVector2();
            //int speed = RandomGenerator.r.Next(1,(int)Config.Default_Bullet_Speed*2);
            double size = RandomGenerator.r.Next((int)Config.Default_Bullet_Size_MIN, (int)Config.Default_Bullet_Size_MAX);
            this.Ctor_helper(destination, current_LocationX, current_LocationY, size);
        }

        private void Ctor_helper(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, double size = Config.Default_Bullet_Size_MAX)
        {
            this.IsHarmful = true;
            this.lines = new Line[5];
            this.current_Location = new My_Coordinates(start_Location_x, start_Location_y);

            this.Destination = destination;
            this.size = size;
            this.ID = idCounter;
            idCounter++;
            NextLocationCalculator();
            NextLocationLinesCalculator();



        }



        public void OneStep()
        {

          
           


            if (next_location.X< Config.Default_Map_size_X && next_location.X>=0 && next_location.Y < Config.Default_Map_size_Y && next_location.Y>=0)
            {
                this.current_Location = next_location;
            }
            else
            {
                BulletreRandomregeneration();
            }
            NextLocationCalculator();
            NextLocationLinesCalculator();

        }

        private void NextLocationLinesCalculator()
        {
            //topleft
            this.lines[0] = (My_Coordinates.LineFromTwoPoints(this.current_Location, this.next_location));
            //topright
            this.lines[1] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size,this.current_Location.Y), new My_Coordinates(this.next_location.X + (int)this.size, this.next_location.Y)));
            //bottomleft
            
            this.lines[2] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X, this.current_Location.Y + (int)this.size), new My_Coordinates(this.next_location.X, this.next_location.Y + (int)this.size)));
            //bottom right
            this.lines[3] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size), new My_Coordinates(this.next_location.X + (int)this.size, this.next_location.Y + (int)this.size)));

            this.lines[4] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size/2, this.current_Location.Y + (int)this.size/2), new My_Coordinates(this.next_location.X + (int)this.size/2, this.next_location.Y + (int)this.size/2)));
        }
        private void NextLocationCalculator()
        {
            this.next_location = new My_Coordinates(this.current_Location.X + (int)destination.X, this.current_Location.Y + (int)destination.Y);
        }


        public override string ToString()
        {
            return $"ID: {ID}\nDestination: {Destination}\nSpeed: {Speed}\nSize: {size}" ;
        }

        private void BulletreRandomregeneration()
        {

            this.current_Location = new My_Coordinates(Config.Default_Bullet_Location_x, Config.Default_Bullet_Location_y);//.GenerateRandomCoordinate();
            this.destination = My_Coordinates.GenerateRandomVector2();
            this.IsHarmful = true;
        }


    }
}
