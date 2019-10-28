using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;

namespace version_3D
{
    public class Bullet3D
    {
        static int idCounter = Config.RobotID + 1;
        int id;
        public My_Coordinates current_Location;
        public My_Coordinates next_location;
        public Line[] next_location_lines;
        public Line[] destination_lines;
        public double size;
        Vector2 destination;

        public My_Coordinates Current_Location => this.current_Location;

        public bool IsHarmful { get; set; }

        public double Speed { get => this.destination.Length(); }
        public Vector2 Destination { get => destination; set => destination = value; }
        public int ID { get => id; set => id = value; }

        public Bullet3D()
        {

        }

        public Bullet3D(Vector2 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size_MAX)
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
            this.next_location_lines = new Line[5];
            this.destination_lines = new Line[5];
            this.current_Location = new My_Coordinates(start_Location_x, start_Location_y);

            this.Destination = destination;
            this.size = size;
            this.ID = idCounter;
            idCounter++;
            NextLocationCalculator();
            NextLocationLinesCalculator();
            DestinationLinesCalculator();





        }


        public void OneStep()
        {

            if (next_location.X < Config.Default_Map_size_X && next_location.X >= 0 && next_location.Y < Config.Default_Map_size_Y && next_location.Y >= 0)
            {
                this.current_Location = next_location;
            }
            else
            {
                BulletreRandomregeneration();
            }
            NextLocationCalculator();

            NextLocationLinesCalculator();
            DestinationLinesCalculator();

        }

        private void NextLocationLinesCalculator()
        {

            //topleft
            this.next_location_lines[0] = (My_Coordinates.LineFromTwoPoints(this.current_Location, this.next_location));
            //topright
            this.next_location_lines[1] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y), new My_Coordinates(this.next_location.X + (int)this.size, this.next_location.Y)));
            //bottomleft

            this.next_location_lines[2] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X, this.current_Location.Y + (int)this.size), new My_Coordinates(this.next_location.X, this.next_location.Y + (int)this.size)));
            //bottom right
            this.next_location_lines[3] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size), new My_Coordinates(this.next_location.X + (int)this.size, this.next_location.Y + (int)this.size)));

            this.next_location_lines[4] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2), new My_Coordinates(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2)));
        }

        private void DestinationLinesCalculator()
        {
            //FOR TEST
            // double distance = (Math.Pow(this.current_Location.X - this.next_location.X, 2) + Math.Pow(this.current_Location.Y - this.next_location.Y, 2));
            double distance = (Math.Pow(this.destination.X, 2) + Math.Pow(this.destination.Y, 2));
            double multipliera = (Math.Sqrt(Math.Pow(Config.Default_Map_size_X, 2) + Math.Pow(Config.Default_Map_size_Y, 2)));

            double multiplier = multipliera;

            My_Coordinates destinationpoint = new My_Coordinates(this.next_location.X + (int)(destination.X * multiplier), this.next_location.Y + (int)(destination.Y * multiplier));



            //this.destination_lines = ArrayExtensions.GetDeepCopy(this.next_location_lines);

            //for (int i = 0; i < this.destination_lines.Length; i++)
            //{
            //    this.destination_lines[i].X2 = this.next_location_lines[i].X2 * 3 ;
            //    this.destination_lines[i].Y2 = this.next_location_lines[i].Y2 * 3;
            //}

            //topleft
            this.destination_lines[0] = (My_Coordinates.LineFromTwoPoints(this.current_Location, destinationpoint));
            //topright
            this.destination_lines[1] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y), new My_Coordinates(destinationpoint.X + (int)this.size, destinationpoint.Y)));
            //bottomleft

            this.destination_lines[2] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X, this.current_Location.Y + (int)this.size), new My_Coordinates(destinationpoint.X, destinationpoint.Y + (int)this.size)));
            //bottom right
            this.destination_lines[3] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size), new My_Coordinates(destinationpoint.X + (int)this.size, destinationpoint.Y + (int)this.size)));

            this.destination_lines[4] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2), new My_Coordinates(destinationpoint.X + (int)this.size / 2, destinationpoint.Y + (int)this.size / 2)));


        }

        private void NextLocationCalculator()
        {
            this.next_location = new My_Coordinates(this.current_Location.X + (int)destination.X, this.current_Location.Y + (int)destination.Y);
        }


        public override string ToString()
        {
            return $"ID: {ID}\nDestination: {Destination}\nSpeed: {Speed}\nSize: {size}";
        }

        private void BulletreRandomregeneration()
        {

            this.current_Location = new My_Coordinates(Config.Default_Bullet_Location_x, Config.Default_Bullet_Location_y);//.GenerateRandomCoordinate();
            this.destination = My_Coordinates.GenerateRandomVector2();
            DestinationLinesCalculator();
            this.IsHarmful = true;
        }


    }
}
