using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_version
{
    interface IMapObject
    {
        double[] Current_Location { get; }
        bool IsHarmful { get; }
    }
}
