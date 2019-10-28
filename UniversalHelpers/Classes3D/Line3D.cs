using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace UniversalHelpers.Classes3D
{
    public class Line3D 
    {
        int x1;
        int y1;
        int z1;

        int x2;
        int y2;
        int z2;

        public int X1 { get => x1; set => x1 = value; }
        public int Y1 { get => y1; set => y1 = value; }
        public int Z1 { get => z1; set => z1 = value; }
        public int X2 { get => x2; set => x2 = value; }
        public int Y2 { get => y2; set => y2 = value; }
        public int Z2 { get => z2; set => z2 = value; }
    }
}
