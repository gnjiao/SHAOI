using DealConfigFile;
using DealRobot;
using ModulePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public partial class Protocols
    {
        /// <summary>
        /// 插栏相机朝向
        /// </summary>
        public static DirCstCamera_Enum DirPhoto => DirCstCamera_Enum.Backward;
        /// <summary>
        /// 插栏z轴补偿轴
        /// </summary>
        public static TypeModuleZ_Enum DirZ => TypeModuleZ_Enum.ModuleUp;
        /// <summary>
        /// 插栏方向
        /// </summary>
        public static DirInsert_Enum DirInsert => DirInsert_Enum.PToN;
        /// <summary>
        /// 插栏相机画面显示是否镜像
        /// </summary>
        public static bool CstIsMirrorX => ConfigManager.instance.CstIsMirrorX;
        /// <summary>
        /// 根据机器人型号，确定厚度补偿方向
        /// </summary>
        static double ThicknessOffset
        {
            get
            {
                double coef = 1;
                switch (ParSetRobot.P_I.TypeRobot_e)
                {
                    case TypeRobot_enum.Epsion_Ethernet:
                        coef = 1;
                        break;
                    case TypeRobot_enum.Epsion_Serial:
                        coef = 1;
                        break;
                    case TypeRobot_enum.YAMAH_Ethernet:
                        coef = -1;
                        break;
                    case TypeRobot_enum.YAMAH_Serial:
                        coef = -1;
                        break;
                }
                return coef * confGlassThicknes;
            }
        }

        /// <summary>
        /// 配方-玻璃X
        /// </summary>
        public static double confGlassX
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassX].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃Y
        /// </summary>
        public static double confGlassY
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassY].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃厚度
        /// </summary>
        public static double confGlassThicknes
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Thickness].DblValue;
            }
        }

        public static double confDisMark
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisMark].DblValue;                
            }
        }

        public static double confMarkX
        {
            get
            {
                return 0;// ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.MarkX].DblValue;                
            }
        }

        public static double confMarkY
        {
            get
            {
                return 0;// ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.MarkY].DblValue;
            }
        }

        /// <summary>
        /// 配方-龙骨列数
        /// </summary>
        public static int KeelCol
        {
            get
            {
                return confCSTCol + 1;
            }
        }

        public static int confLayerSpacing
        {
            get=> (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.LayerSpacing].DblValue;
        }

        /// <summary>
        /// 配方-插栏列数
        /// </summary>
        public static int confCSTCol
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTCols].DblValue;
            }
        }
        /// <summary>
        /// 配方-插栏行数
        /// </summary>
        public static int confCSTRow
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTRows].DblValue;
            }
        }
        /// <summary>
        /// 配方-龙骨间距
        /// </summary>
        public static double confKeelInterval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.KeelInterval].DblValue;
            }
        }
        /// <summary>
        /// 配方-第一列龙骨位置
        /// </summary>
        public static double confCol1Interval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisCol1].DblValue;
            }
        }
    }
}
