using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    #region robot std
    public enum BotStd
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos,
    }
    #endregion

    #region robot adj
    public enum BotAdj
    {
        /// <summary>
        /// 粗定位取片位置
        /// </summary>
        PickPos = 1,
        /// <summary>
        /// 精定位位置
        /// </summary>
        PrecisePos,
        /// <summary>
        /// 抛料位置
        /// </summary>
        DumpPos,
    }
    #endregion

    /// <summary>
    /// 配方寄存器 d5020
    /// </summary>
    public enum RecipeRegister
    {
        /// <summary>
        /// 玻璃X
        /// </summary>
        GlassX,
        /// <summary>
        /// 玻璃Y
        /// </summary>
        GlassY,
        /// <summary>
        /// 玻璃厚度
        /// </summary>
        Thickness,
        /// <summary>
        /// 工位放片方向
        /// </summary>
        DIR_PLACE,
        /// <summary>
        /// 精定位方向
        /// </summary>
        DIR_PRECISE,
        /// <summary>
        /// Mark间距
        /// </summary>
        DisMark,
        /// <summary>
        /// 龙骨层间距
        /// </summary>
        LayerSpacing = 9,
        /// <summary>
        /// 第一列龙骨间距
        /// </summary>
        DisCol1,
        /// <summary>
        /// 龙骨间距
        /// </summary>
        KeelInterval,
        /// <summary>
        /// 卡塞行数
        /// </summary>
        CSTRows = 15,
        /// <summary>
        /// 卡塞列数
        /// </summary>
        CSTCols,
        /// <summary>
        /// 抛料方向
        /// </summary>
        DIR_Dump,
    }

    /// <summary>
    /// 数据寄存器1，d1200
    /// </summary>
    public enum DataRegister1
    {
        PCAlarm = 1,
        /// <summary>
        /// 皮带线系数
        /// </summary>
        BeltRatioY = 4,
        /// <summary>
        /// 当前插栏总数
        /// </summary>
        CurrentInsertSum,
        /// <summary>
        /// 插栏数据确认
        /// </summary>
        InsertDataConfirm,
    }

    /// <summary>
    /// 数据寄存器2，d1250
    /// </summary>
    public enum DataRegister2
    {
        /// <summary>
        /// 插栏1基准值
        /// </summary>
        StdCSTPos1,
        /// <summary>
        /// 插栏2基准值
        /// </summary>
        StdCSTPos2,
        /// <summary>
        /// 插栏3基准值
        /// </summary>
        StdCSTPos3,
        /// <summary>
        /// 插栏4基准值
        /// </summary>
        StdCSTPos4,
    }

    /// <summary>
    /// 数据寄存器3，d1300
    /// </summary>
    public enum DataRegister3
    {
        InsertStdCom,
        InsertData,
        InsertComZ1,
        KeelSpacing1 = 12,
    }

    /// <summary>
    /// 报警
    /// </summary>
    public enum PCAlarm_Enum
    {
        标定失败 = 1,
        卡塞计算失败 = 2,
    }
}
