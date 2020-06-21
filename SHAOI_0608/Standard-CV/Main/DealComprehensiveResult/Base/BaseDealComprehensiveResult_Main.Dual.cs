using BasicClass;
using Camera;
using DealCalibrate;
using DealConfigFile;
using DealRobot;
using ModulePackage;
using ParComprehensive;
using StationDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    partial class BaseDealComprehensiveResult_Main
    {
        protected static Point2D Pt2Mark1 = new Point2D();
        protected static Point2D Pt2Mark2 = new Point2D();
        protected static Point2D Pt2MarkRC = new Point2D();

        public static bool Camera1Done = false;
        public static bool Camera2Done = false;

        public void DualLocation(int index)
        {
            if (Camera1Done & Camera2Done)
            {
                ShowState("开始计算工位" + index + "偏差");
                BaseParCalibrate baseParComprehensive = ParComprehensive2.P_I.GetCellParCalibrate(Camera2RC);
                ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;
                double angle = Math.Atan(
                    (Pt2Mark2.DblValue2
                    - Pt2Mark1.DblValue2)
                    * AMP
                    / Protocols.confDisMark) * 180 / Math.PI
                    - StationDataMngr.CalibPos_L[index - 1].DblValue4;
                ShowState("工位" + index + "逆时针角度偏差: " + angle);


                FunCalibRotate fcr = new FunCalibRotate();
                Point2D MarkAfterRotate = fcr.GetPoint_AfterRotation(
                    angle / 180 * Math.PI, parCalibRotate.PointRC, Pt2Mark2);

                double deltaY = MarkAfterRotate.DblValue1 - StationDataMngr.CalibPos_L[index - 1].DblValue1;
                double deltaX = MarkAfterRotate.DblValue2 - StationDataMngr.CalibPos_L[index - 1].DblValue2;
                deltaX *= AMP;
                deltaY *= -AMP;

                Point2D delta = TransDelta(new Point2D(deltaX, deltaY),
                    Protocols.ConfPlaceAngle, Protocols.ConfPreciseAngle);
                ShowState(string.Format("工位{0}X方向补偿{1},Y方向补偿{2}", index, delta.DblValue1.ToString(ReserveDigits), delta.DblValue2.ToString(ReserveDigits)));

                int num = (StationNum - 1) * 2 + index;
                Point4D pos = new Point4D
                {
                    DblValue1 = delta.DblValue1
                    + StationDataMngr.PlacePos_L[index - 1].DblValue1
                    + ParAdjust.Value1("adj" + num),

                    DblValue2 = delta.DblValue2
                    + StationDataMngr.PlacePos_L[index - 1].DblValue2
                    + ParAdjust.Value2("adj" + num),

                    DblValue3 = StationDataMngr.PlacePos_L[index - 1].DblValue3,
                    DblValue4 = Protocols.ConfPlaceAngle + angle + ParAdjust.Value3("adj" + num)
                }; 

                LogicRobot.L_I.WriteRobotCMD(pos, Protocols.BotCmd_StationPos);
                ShowState("发送机器人放片坐标：" + pos.DblValue1.ToString("f3") + @"/" + pos.DblValue2.ToString("f3"));
            }
        }

        public void CalibDual(int index)
        {
            if (Camera1Done & Camera2Done)
            {
                double angle = Math.Atan((Pt2Mark2.DblValue2 - Pt2Mark1.DblValue2) * AMP
                    / Protocols.confDisMark) * 180 / Math.PI;
                ShowState("精定位验证工位" + index + "逆时针角度偏差: " + angle);

                StationDataMngr.CalibPos_L[index - 1].DblValue1 = Pt2Mark2.DblValue1;
                StationDataMngr.CalibPos_L[index - 1].DblValue2 = Pt2Mark2.DblValue2;
                StationDataMngr.CalibPos_L[index - 1].DblValue4 = angle;
                StationDataMngr.WriteIniCalibPos(index);

                StationDataMngr.WriteIniCalibPosLocal(index);

                LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_CalibOK);
            }
        }

        public void CalibRC()
        {
            Thread.Sleep(500);
            //double curR= ModuleBase.GetCurAngleByY(Protocols.GlassXInPresize, AMP, Pt2Mark1, Pt2MarkRC);
            BaseParCalibrate baseParComprehensive = ParComprehensive2.P_I.GetCellParCalibrate(Camera2RC);
            ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;

            Point2D orgPoint = new FunCalibRotate().GetOriginPoint(Protocols.BotRCCalibAngle, Pt2Mark2, Pt2MarkRC);
            ShowState("计算旋转中心X:" + orgPoint.DblValue1 + ",Y:" + orgPoint.DblValue2);
            Point2D offset = new Point2D(Protocols.GlassXInPresize / 2 / AMP, Protocols.GlassYInPresize / 2 / AMP);
            double r = ModuleBase.GetCurAngleByY(Protocols.GlassXInPresize, AMP, Pt2Mark1, Pt2Mark2);
            offset = TransDelta(offset, r + 0.5, 0);
            Point2D rc = new Point2D(offset.DblValue1 + Pt2Mark2.DblValue1, offset.DblValue2 + Pt2Mark2.DblValue2);
            ShowState("理论旋转中心X:" + rc.DblValue1 + ",Y:" + rc.DblValue2);
            if(Math.Abs(orgPoint.DblValue1-rc.DblValue1)<1000 &&
                Math.Abs(orgPoint.DblValue2 - rc.DblValue2) < 1000)
            {
                ShowState("使用计算旋转中心");
                parCalibRotate.XRC = orgPoint.DblValue1;
                parCalibRotate.YRC = orgPoint.DblValue2;
            }
            else
            {
                ShowState("使用理论旋转中心");
                parCalibRotate.XRC = rc.DblValue1;// orgPoint.DblValue1;
                parCalibRotate.YRC = rc.DblValue2;// orgPoint.DblValue2;
            }
            
            ParComprehensive2.P_I.WriteXmlDoc(Camera2RC);
            //将参数保存到结果类
            new FunCalibRotate().SaveParToResult(HtResult_Cam2, parCalibRotate);

            ShowState(string.Format("旋转中心标定完成,X_{0},Y_{1}", parCalibRotate.XRC.ToString(), parCalibRotate.YRC.ToString()));

            LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_CalibRC);
        }
    }
}
