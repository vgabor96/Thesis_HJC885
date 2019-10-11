using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

       

        public static void Set_IMapObjectElement(this double[,] array, double x, double y, MapObjectType value)
        {
            //array[array.GetLength(0) - y, x] = (double)value;
            Set_MapO(array, (int)Math.Round(x), (int)Math.Round(y), value);
        }

        public static void Set_IMapObjectElement(this double[,] array, My_Coordinates myCoord, MapObjectType value)
        {
            //array[array.GetLength(0) - myCoord.Y, myCoord.X] = (double)value;
            Set_MapO(array,myCoord.X, myCoord.Y, value);
        }

        private static void Set_MapO(double[,] array, int x, int y, MapObjectType value)
        {

            //array[array.GetLength(0) - (y+1), x] = (double)value;
            array[array.GetLength(0) - (x + 1), y] = (double)value;

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
    }
}
