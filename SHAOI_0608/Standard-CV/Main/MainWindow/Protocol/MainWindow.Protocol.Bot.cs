using BasicClass;
using DealConfigFile;
using DealImageProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class Protocols
    {
        //public static string BotCmd_Station1 = "11";
        public static string BotCmd_StationPos = "11";
        //public static string BotCmd_Station2 = "12";
        public static string BotCmd_PreciseOK = "13";
        public static string BotCmd_PreciseNG = "19";

        public static string BotCmd_CalibStation = "21";
        public static string BotCmd_CalibOK = "22";
        public static string BotCmd_CalibRC = "23";
        public static string BotCmd_Move = "24";
        public static string BotCmd_Teach = "25";
        public static string BotCmd_TeachOver = "26";

        public static Point4D BotPickPos
        {
            get
            {
                return StdBotPickPos + AdjBotPickPos;
            }
        }

        public static Point4D StdBotPickPos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.PickPos].Add(0, -confGlassY / 2);
            }
        }

        public static Point4D AdjBotPickPos
        { 
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.PickPos];
            }
        }

        public static Point4D StdBotPrecisePos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.PrecisePos].Add(0, -GlassYInPresize / 2).Add(1, -confDisMark / 2).Add(3, Protocols.ConfPreciseAngle);
            }
        }

        public static Point4D BotDumpPos
        {
            get
            {
                return StdBotDumpPos + AdjBotDumpPos;
            }
        }

        public static Point4D StdBotDumpPos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.DumpPos];
            }
        }

        public static Point4D AdjBotDumpPos
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.DumpPos];
            }
        }

        public static int ConfPreciseAngle
        {
            get
            {
                int dir = (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PRECISE].DblValue;
                int angle = 0;
                while (dir > 1)
                {
                    dir >>= 1;
                    angle -= 90;
                }
                return (angle + 360) % 360;
            }
        }

        public static int ConfPlaceAngle
        {
            get
            {
                int dir = (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PLACE].DblValue;
                int angle = 0;
                while (dir > 1)
                {
                    dir >>= 1;
                    angle -= 90;
                }
                return (angle + 360) % 360;
            }
        }

        public static double GlassXInPresize
        {
            get
            {
                return ConfPreciseAngle % 180 == 0 ? confGlassX : confGlassY;
            }
        }

        public static double GlassYInPresize
        {
            get
            {
                return ConfPreciseAngle % 180 == 0 ? confGlassY : confGlassX;
            }
        }

        public static double BotRCCalibAngle = 0.5;
    }
}
