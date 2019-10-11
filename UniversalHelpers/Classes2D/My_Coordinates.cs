using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
