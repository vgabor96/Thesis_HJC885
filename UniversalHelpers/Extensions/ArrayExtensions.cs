using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Enumerators;

namespace UniversalHelpers.Extensions
{
    public static class ArrayExtensions
    {
        public static bool DoesContainsThisCoordinate(this double[,] array, My_Coordinates myCoord )
        {
            return (0 <= myCoord.X && myCoord.X <array.GetLength(1) && 0 <= myCoord.Y && myCoord.Y < array.GetLength(0) ) ? true : false;
        }

        public static double Get_IMapObjectElement(this double[,] array, My_Coordinates myCoord)
        {
            return array[array.GetLength(0) - (myCoord.Y+1), myCoord.X];
        }

        public static double Get_IMapObjectElement(this double[,] array, int x, int y)
        {
            return array[array.GetLength(0) - y, x];
        }

       

        public static void Set_IMapObjectElement(this double[,] array, double x, double y, double ID)
        {
            //array[array.GetLength(0) - y, x] = (double)value;
            Set_MapO(array, (int)Math.Round(x), (int)Math.Round(y), ID);
        }

        public static void Set_IMapObjectElement(this double[,] array, My_Coordinates myCoord, double ID)
        {
            //array[array.GetLength(0) - myCoord.Y, myCoord.X] = (double)value;
            Set_MapO(array,myCoord.X, myCoord.Y, ID);
        }

        private static void Set_MapO(double[,] array, int x, int y, double ID)
        {

            //array[array.GetLength(0) - (y+1), x] = (double)value;
            array[array.GetLength(0) - (x + 1), y] = ID;

        }

        public static string MapToString(this double[,] array)
        {
            string s = "";
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i,j].Equals(0))
                    {
                        s += " ";
                    }
                    else
                    {
                        s += array[i,j];
                    }
                   
                }
                s += "\n";
            }

            return s;
        }

        public static Line[] GetDeepCopy(this Line[] lines)
        {
            Line[] result = new Line[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = new Line()
                {
                    X1 = lines[i].X1,
                    X2 = lines[i].X1,
                    Y1 = lines[i].Y1,
                    Y2 = lines[i].Y2
                };
            }

            return result;
        }

        public static Line GetDeepCopy_Line(this Line line)
        {
            return new Line()
            {
                X1 = line.X1,
                X2 = line.X1,
                Y1 = line.Y1,
                Y2 = line.Y2
            };
        }

    }
}
