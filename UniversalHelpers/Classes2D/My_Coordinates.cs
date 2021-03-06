﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using UniversalHelpers.Configurations;

namespace UniversalHelpers.Classes2D
{
    public class My_Coordinates
    {
        private int x;
        private int y;

        public int X { get => this.x; set => this.x = value; }
        public int Y { get => this.y; set => this.y = value; }

        public My_Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void GenerateRandomCoordinate()
        {
            
            this.x = RandomGenerator.r.Next(0, Config.Default_Map_size_X);
            this.y = RandomGenerator.r.Next(0, Config.Default_Map_size_Y);
        }

        public static Vector2 GenerateRandomVector2()
        {
            float v1 = 0;
            float v2 = 0;

            v1 = (float)RandomGenerator.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));
           
            //Vectors should moving => both of them cannot be 0.
            while (v1 == 0)
            {
                v1 = (float)RandomGenerator.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

            }
            v2 = (float)RandomGenerator.r.Next(Convert.ToInt32(Config.Default_Vector_from), Convert.ToInt32(Config.Default_Vector_to));

                return new Vector2(v1, v2); 
        }

        public static Line LineFromTwoPoints(My_Coordinates first, My_Coordinates second)
        {
            Line line = new Line()
            {
                X1 = first.X,
                X2 = second.X,
                Y1 = first.Y,
                Y2 = second.Y
            };
            return line;
      
        }
        public static bool DoesLineContainPoint(Line line, My_Coordinates point, double linewidth = 0)
        {


            if (linewidth == 0)
            {
                bool first = point.X >= Math.Min(line.X1, line.X2);
                bool sec = point.X <= Math.Max(line.X1, line.X2);
                bool third = point.Y >= Math.Min(line.Y1, line.Y2);
                bool fou = point.Y <= Math.Max(line.Y1, line.Y2);
                if (first)
                {
                    if (sec)
                    {
                        if (third)
                        {
                            if (fou)
                            {

                            }
                        }
                    }
                }
                if (point.X >= Math.Min(line.X1,line.X2) &&
                    point.X <= Math.Max(line.X1,line.X2) &&
                    point.Y >= Math.Min(line.Y1,line.Y2) &&
                    point.Y <= Math.Max(line.Y1,line.Y2))
                {
                    return true;
                }
            }
            else
            {
                double width = linewidth / 2;
                if (point.X >= Math.Min(line.X1, line.X2) && point.X <= Math.Max(line.X1, line.X2) && point.Y >= Math.Min(line.Y1, line.Y2) && point.Y <= Math.Max(line.Y1, line.Y2))
                {
                    return true;
                }

            }
            return false;
        }
    }
}
