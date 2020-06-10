using BasicClass;
using DealConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class Protocols
    {
        const string RefrenceStation1 = "adj1";
        const string RefrenceStation2 = "adj2";

        public static double[] AdjX_Station = new double[2] 
        { ParAdjust.Value1(RefrenceStation1),
        ParAdjust.Value1(RefrenceStation2)};

        public static double[] AdjY_Station = new double[2]
        { ParAdjust.Value2(RefrenceStation1),
        ParAdjust.Value2(RefrenceStation2)};

        public static double[] AdjR_Station = new double[2]
        { ParAdjust.Value3(RefrenceStation1),
        ParAdjust.Value3(RefrenceStation2)};
        
    }
}
