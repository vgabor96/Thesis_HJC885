using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Classes3D;
using UniversalHelpers.Configurations;

namespace version_3D
{
    public class Bullet3D
    {
        static int idCounter = Config.RobotID + 1;
        int id;
        public My_Coordinates3D current_Location;
        public My_Coordinates3D next_location;
        public Line3D[] next_location_lines;
        public Line3D[] destination_lines;
        public double size;
        Vector3 destination;

        public My_Coordinates3D Current_Location => this.current_Location;

        public bool IsHarmful { get; set; }

        public double Speed { get => this.destination.Length(); }
        public Vector3 Destination { get => destination; set => destination = value; }
        public int ID { get => id; set => id = value; }

        public Bullet3D()
        {

        }

        public Bullet3D(Vector3 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, int start_Location_z = Config.Default_Bullet_Location_z, double speed = Config.Default_Bullet_Speed, double size = Config.Default_Bullet_Size_MAX)
        {
            this.Ctor_helper(destination, start_Location_x, start_Location_y, start_Location_z, size);
        }

        public void GenerateRandomBullet()
        {

            int current_LocationX = Config.Default_Bullet_Location_x;//RandomGenerator.r.Next(0, Config.Default_Map_size_X);
            int current_LocationY = Config.Default_Bullet_Location_y;//RandomGenerator.r.Next(0, Config.Default_Map_size_Y);
            int current_LocationZ = Config.Default_Bullet_Location_z;
            Vector3 destination = My_Coordinates3D.GenerateRandomVector3();
            //int speed = RandomGenerator.r.Next(1,(int)Config.Default_Bullet_Speed*2);
            double size = RandomGenerator.r.Next((int)Config.Default_Bullet_Size_MIN, (int)Config.Default_Bullet_Size_MAX);
            this.Ctor_helper(destination, current_LocationX, current_LocationY, current_LocationZ,  size);
        }

        private void Ctor_helper(Vector3 destination, int start_Location_x = Config.Default_Bullet_Location_x, int start_Location_y = Config.Default_Bullet_Location_y, int start_Location_z = Config.Default_Bullet_Location_z , double size = Config.Default_Bullet_Size_MAX)
        {
            this.IsHarmful = true;
            this.next_location_lines = new Line3D[27];
            this.destination_lines = new Line3D[27];
            this.current_Location = new My_Coordinates3D(start_Location_x, start_Location_y, start_Location_z);

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

            ////topleft
            //this.next_location_lines[0] = (My_Coordinates3D.LineFromTwoPoints(this.current_Location, this.next_location));
            ////topright
            //this.next_location_lines[1] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.Current_Location.X + (int)this.size, this.current_Location.Y), new My_Coordinates3D(this.next_location.X + (int)this.size, this.next_location.Y)));
            ////bottomleft

            //this.next_location_lines[2] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.Current_Location.X, this.current_Location.Y + (int)this.size), new My_Coordinates3D(this.next_location.X, this.next_location.Y + (int)this.size)));
            ////bottom right
            //this.next_location_lines[3] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.Current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size), new My_Coordinates3D(this.next_location.X + (int)this.size, this.next_location.Y + (int)this.size)));

            //this.next_location_lines[4] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.Current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2)));

            this.NextLocationCalculator_Loop();
        }


        ////Middlepoints
        //private void NextLocationLinesCalculator_From1_To6()
        //{
        //    this.next_location_lines[0] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X+(int)this.size/2,this.current_Location.Y+(int)this.size/2,this.current_Location.Z), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2, this.next_location.Z)));

        //    this.next_location_lines[1] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size/2), new My_Coordinates3D(this.next_location.X, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size / 2)));



        //    this.next_location_lines[2] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X+ (int)this.size/2, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size)));

        //    this.next_location_lines[3] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size), new My_Coordinates3D(this.next_location.X + (int)this.size, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size)));


        //    this.next_location_lines[4] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size/2, this.current_Location.Y, this.current_Location.Z + (int)this.size/2), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y, this.next_location.Z + (int)this.size / 2)));



        //    this.next_location_lines[5] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size, this.current_Location.Z + (int)this.size / 2), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size, this.next_location.Z + (int)this.size / 2)));

        //}

        ////private void NextLocationCalculator_From7_To14()
        ////{
        ////    this.next_location_lines[6] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2, this.next_location.Z)));

        ////    this.next_location_lines[7] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size / 2), new My_Coordinates3D(this.next_location.X, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size / 2)));



        ////    this.next_location_lines[8] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size)));

        ////    this.next_location_lines[9] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size / 2, this.current_Location.Z + (int)this.size), new My_Coordinates3D(this.next_location.X + (int)this.size, this.next_location.Y + (int)this.size / 2, this.next_location.Z + (int)this.size)));


        ////    this.next_location_lines[10] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size / 2, this.current_Location.Y, this.current_Location.Z + (int)this.size / 2), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y, this.next_location.Z + (int)this.size / 2)));



        ////    this.next_location_lines[11] = (My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D(this.current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size, this.current_Location.Z + (int)this.size / 2), new My_Coordinates3D(this.next_location.X + (int)this.size / 2, this.next_location.Y + (int)this.size, this.next_location.Z + (int)this.size / 2)));


        ////}

        private void NextLocationCalculator_Loop()
        {
            int index = 0;

            for (int i = 0; i < this.next_location_lines.Length / 9; i++)
            {


                for (int j = 0; j < this.next_location_lines.Length / 9; j++)
                {
                    for (int k = 0; k < this.next_location_lines.Length / 9; k++)
                    {
                        this.next_location_lines[index] = My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D((int)(size * (i / 2)) + this.current_Location.X, (int)(size * (j / 2)) + this.current_Location.Y, (int)(size * (k / 2)) + this.current_Location.Z), new My_Coordinates3D((int)(size * (i / 2)) + this.next_location.X, (int)(size * (j / 2)) + this.next_location.Y, (int)(size * (k / 2)) + this.next_location.Z));
                        index++;
                        
                    }
                }


            }


        }


        private void DestinationLinesCalculator()
            {

            DestinationCalculator_Loop();
            //FOR TEST
            // double distance = (Math.Pow(this.current_Location.X - this.next_location.X, 2) + Math.Pow(this.current_Location.Y - this.next_location.Y, 2));


            ////topleft
            //this.destination_lines[0] = (My_Coordinates.LineFromTwoPoints(this.current_Location, destinationpoint));
            ////topright
            //this.destination_lines[1] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y), new My_Coordinates(destinationpoint.X + (int)this.size, destinationpoint.Y)));
            ////bottomleft

            //this.destination_lines[2] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X, this.current_Location.Y + (int)this.size), new My_Coordinates(destinationpoint.X, destinationpoint.Y + (int)this.size)));
            ////bottom right
            //this.destination_lines[3] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size, this.current_Location.Y + (int)this.size), new My_Coordinates(destinationpoint.X + (int)this.size, destinationpoint.Y + (int)this.size)));

            //this.destination_lines[4] = (My_Coordinates.LineFromTwoPoints(new My_Coordinates(this.Current_Location.X + (int)this.size / 2, this.current_Location.Y + (int)this.size / 2), new My_Coordinates(destinationpoint.X + (int)this.size / 2, destinationpoint.Y + (int)this.size / 2)));


        }


        private void DestinationCalculator_Loop()
        {
            double distance = Math.Sqrt((Math.Pow(this.destination.X, 2) + Math.Pow(this.destination.Y, 2) + Math.Pow(this.destination.Z, 2)));
            double multipliera = (Math.Sqrt(Math.Pow(Config.Default_Map_size_X, 2) + Math.Pow(Config.Default_Map_size_Y, 2)));

            double multiplier = multipliera;

            My_Coordinates3D destinationpoint = new My_Coordinates3D(this.next_location.X + (int)(destination.X * multiplier), this.next_location.Y + (int)(destination.Y * multiplier), this.next_location.Z + (int)(destination.Z * multiplier));

            int index = 0;

            for (int i = 0; i < this.destination_lines.Length / 9; i++)
            {


                for (int j = 0; j < this.destination_lines.Length / 9; j++)
                {
                    for (int k = 0; k < this.destination_lines.Length / 9; k++)
                    {
                        this.destination_lines[index] = My_Coordinates3D.LineFromTwoPoints(new My_Coordinates3D((int)(size * (i / 2)) + this.current_Location.X, (int)(size * (j / 2)) + this.current_Location.Y, (int)(size * (k / 2)) + this.current_Location.Z), new My_Coordinates3D((int)(size * (i / 2)) +destinationpoint.X, (int)(size * (j / 2)) + destinationpoint.Y, (int)(size * (k / 2)) + destinationpoint.Z));
                        index++;
                    }
                }


            }


        }

        private void NextLocationCalculator()
        {
            this.next_location = new My_Coordinates3D(this.current_Location.X + (int)destination.X, this.current_Location.Y + (int)destination.Y, this.current_Location.Z + (int)destination.Z);
        }


        public override string ToString()
        {
            return $"ID: {ID}\nDestination: {Destination}\nSpeed: {Speed}\nSize: {size}";
        }

        private void BulletreRandomregeneration()
        {

            this.current_Location = new My_Coordinates3D(Config.Default_Bullet_Location_x, Config.Default_Bullet_Location_y, Config.Default_Bullet_Location_z);//.GenerateRandomCoordinate();
            this.destination = My_Coordinates3D.GenerateRandomVector3();
            DestinationLinesCalculator();
            this.IsHarmful = true;
        }


    }
}
