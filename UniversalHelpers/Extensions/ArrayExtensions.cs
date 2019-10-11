using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;

namespace UniversalHelpers.Extensions
{
    public static class ArrayExtensions
    {
        public static bool DoesContainsThisCoordinate(this double[,] array, My_Coordinates myCoord )
        {
            return (0 <= myCoord.X && myCoord.X <array.GetLength(1) && 0 <= myCoord.Y && myCoord.Y < array.GetLength(0) ) ? true : false;
        }
    }
}
