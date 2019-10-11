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
            return array[array.GetLength(0) - myCoord.Y, myCoord.X];
        }

        public static double Get_IMapObjectElement(this double[,] array, int x, int y)
        {
            return array[array.GetLength(0) - y, x];
        }

        public static void Set_IMapObjectElement(this double[,] array, int x, int y, MapObjectType value)
        {
            //array[array.GetLength(0) - y, x] = (double)value;
            Set_MapO(array, x, y, value);
        }

        public static void Set_IMapObjectElement(this double[,] array, My_Coordinates myCoord, MapObjectType value)
        {
            //array[array.GetLength(0) - myCoord.Y, myCoord.X] = (double)value;
            Set_MapO(array,myCoord.X, myCoord.Y, value);
        }

        private static void Set_MapO(double[,] array, int x, int y, MapObjectType value)
        {
            array[array.GetLength(0) - y, x] = (double)value;
        }
    }
}
