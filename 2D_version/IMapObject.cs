using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;

namespace version_2D
{
    interface IMapObject
    {
        My_Coordinates Current_Location { get; }
        bool IsHarmful { get; }
    }
}
