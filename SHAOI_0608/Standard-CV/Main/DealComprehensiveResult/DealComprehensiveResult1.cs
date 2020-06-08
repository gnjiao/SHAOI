using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealImageProcess;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;
using DealGrabImage;
using DealAlgorithm;
using DealLog;

namespace Main
{
    public partial class DealComprehensiveResult1 : BaseDealComprehensiveResult_Main
    {
        #region 定义
        //double            



        #endregion 定义

        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        public override StateComprehensive_enum DealComprehensiveResultFun1(
            TriggerSource_enum trigerSource_e,int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos1;//当前位置
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState("相机1空跑默认OK"); 
                    return StateComprehensive_enum.True;
                }

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera1Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机1拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark1.DblValue1 = result.X;
                Pt2Mark1.DblValue2 = result.Y;
                Camera1Done = true;
                DualLocation(index);

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录

                //g_UCDisplayCamera.ShowResult("NG\nX=20\nY=100\n", false);
                //long t = g_UCDisplayCamera.SaveBitFullPath_Screen("Came","D:\\");
                //t = g_UCDisplayCamera.SaveBit_Screen("D:\\");
                //ShowState_Hidden("保存屏幕截图时间:"+t.ToString());

                //ShowResult_Camera1("1",true);
                //ShowResult_Camera2("2", true);
                //ShowResult_Camera3("3", true);
                //ShowResult_Camera4("4", true);
                //ShowResult_Camera5("5", true);
                //ShowResult_Camera6("6", true);
            }
        }

        #region 各相机转心 Point2D
        /// <summary>
        /// 相机1旋转中心
        /// </summary>
        public static Point2D PRCCam1
        {
            get
            {
                List<string> nameCell = ParComprehensive1.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    return new Point2D(ParAdjust.Value1("adj18"), ParAdjust.Value2("adj18"));
                }
                else
                {
                    ParCalibRotate par = ParComprehensive1.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    return par.PointRC;
                }
            }
            set
            {
                List<string> nameCell = ParComprehensive1.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    ParAdjust.SetValue1("adj18", value.DblValue1);
                    ParAdjust.SetValue2("adj18", value.DblValue2);
                }
                else
                {
                    ParCalibRotate par = ParComprehensive1.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    if (par != null)
                    {
                        par.XRC = value.DblValue1;
                        par.YRC = value.DblValue2;
                        ParComprehensive1.P_I.WriteXmlDoc(par.NameCell);
                    }
                }

            }
        }

        /// <summary>
        /// 相机2旋转中心
        /// </summary>
        public static Point2D PRCCam2 = new Point2D();

        /// <summary>
        /// 相机3旋转中心
        /// </summary>
        public static Point2D PRCCam3 = new Point2D();

        /// <summary>
        /// 相机4旋转中心
        /// </summary>
        public static Point2D PRCCam4
        {
            get
            {
                List<string> nameCell = ParComprehensive4.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    return new Point2D(ParAdjust.Value1("adj18"), ParAdjust.Value2("adj18"));
                }
                else
                {
                    ParCalibRotate par = ParComprehensive4.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    return par.PointRC;
                }
            }
            set
            {
                List<string> nameCell = ParComprehensive4.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    ParAdjust.SetValue1("adj18", value.DblValue1);
                    ParAdjust.SetValue2("adj18", value.DblValue2);
                }
                else
                {
                    ParCalibRotate par = ParComprehensive4.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    if (par != null)
                    {
                        par.XRC = value.DblValue1;
                        par.YRC = value.DblValue2;
                        ParComprehensive4.P_I.WriteXmlDoc(par.NameCell);
                    }
                }

            }
        }
        #endregion
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(
            TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos2;//当前位置
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {
                //标定基准位
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera1Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机1第二次拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark1.DblValue1 = result.X;
                Pt2Mark1.DblValue2 = result.Y;
                Camera1Done = true;
                CalibDual(index);

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
               
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录

            }
        }
        #endregion 位置2拍照

        #region 位置3拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 3;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                //旋转中心
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos3, out htResult);

                ResultBlob result = htResult["C15"] as ResultBlob;
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos3, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 4;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos4, out htResult);

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos4, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照
    }
}
