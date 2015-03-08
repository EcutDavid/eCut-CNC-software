using EcutController;
using Microsoft.Win32;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Utility;
using System.Linq;

namespace P1S1
{
    public partial class MainWindow : Window
    {
        InfoBorad infoBorad;
        InfoBorad gCodeBorad;
        IEcutService cutService;
        GLNumber glBindNumber;
        GLEntity GLEntity;

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            MainWindow win = new MainWindow();
            app.Run(win);
        }

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer textDisplayTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            InitControls();
            cutService = new EcutEntity();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Start();
            textDisplayTimer.Interval = TimeSpan.FromSeconds(2);
            textDisplayTimer.Start();
            SetGLRangeBinding();

            GlHandelInit();
        }

        private void GlHandelInit()
        {
            RotateAddBt.AddHandler(Button.MouseDownEvent, new RoutedEventHandler((sender, e) =>
            {
                OnRotating = true;
                timer.Tick += GlAddAngleTick;
            }), true);
            RotateAddBt.AddHandler(Button.MouseUpEvent, new RoutedEventHandler((sender, e) =>
            {
                timer.Tick -= GlAddAngleTick;
                OnRotating = false;
            }), true);
            RotateSubBt.AddHandler(Button.MouseDownEvent, new RoutedEventHandler((sender, e) =>
            {
                OnRotating = true;
                timer.Tick += GlSubAngleTick;
            }), true);
            RotateSubBt.AddHandler(Button.MouseUpEvent, new RoutedEventHandler((sender, e) =>
            {
                timer.Tick -= GlSubAngleTick;
                OnRotating = false;
            }), true);
        }

        private void SetGLRangeBinding()
        {
            glBindNumber = new GLNumber();
            var bindSetting = new Binding("Number");
            bindSetting.Source = glBindNumber;
            bindSetting.Mode = BindingMode.TwoWay;
            GLRange.SetBinding(TextBox.TextProperty, bindSetting);
            glBindNumber.Number = "200";
        }

        private void InitControls()
        {
            infoBorad = new InfoBorad(MessageBox);
            gCodeBorad = new InfoBorad(GcodeBox);
            initIOLed();
            bindingManualControlModeRate();
            initAxisPos();
        }

        private void AxistTick(object sender, EventArgs e)
        {
            var positon = cutService.MachinePostion;

            for (int i = 0; i < 4; i++)
            {
                AxisNumbers[i].Value = positon[i];
            }
        }

        bindingNumber[] AxisNumbers = new bindingNumber[4];
        private void initAxisPos()
        {
            var textBlockArray = new TextBlock[4];
            textBlockArray[0] = XAxisPosArea;
            textBlockArray[1] = YAxisPosArea;
            textBlockArray[2] = ZAxisPosArea;
            textBlockArray[3] = AAxisPosArea;
            for (int i = 0; i < 4; i++)
            {
                AxisNumbers[i] = new bindingNumber();
                var axisBingSetting = new Binding("DisplayNumber");
                axisBingSetting.Source = AxisNumbers[i];
                textBlockArray[i].SetBinding(TextBlock.TextProperty, axisBingSetting);
            }
        }

        bindingNumber manualMoveRate;
        private void bindingManualControlModeRate()
        {
            manualMoveRate = new bindingNumber();
            var sliderBindSetting = new Binding("Value");
            sliderBindSetting.Source = manualMoveRate;
            ManualMoveSlider.SetBinding(Slider.ValueProperty, sliderBindSetting);

            var textBlockBindSetting = new Binding("Percentage");
            textBlockBindSetting.Source = manualMoveRate;
            ManualMoveRateTextBlock.SetBinding(TextBlock.TextProperty, textBlockBindSetting);
            manualMoveRate.Value = 5;
        }

        ushort OutPutValStore;
        void LedTimerTick(object sender, EventArgs e)
        {

            if (ledManager != null)
            {
                ushort OutPutVal = 0;
                int? InPutVal = 0;
                for (ushort i = 16; i < 32; i++)
                {
                    OutPutVal += (ledManager[i].EnableFlag) ? ((ushort)(1 << (i - 16))) : (ushort)0;
                }
                if (OutPutValStore != OutPutVal)
                {
                    cutService.OutputIO = OutPutVal;
                    OutPutValStore = OutPutVal;
                }
                InPutVal = cutService.InputIO;
                if (InPutVal != null)
                {
                    for (ushort i = 0; i < 16; i++)
                        ledManager[i].EnableFlag = ((InPutVal & (1 << i)) == 0) ? false : true;
                }
            }
        }

        const int LEDCount = 32;
        LEDManager[] ledManager;
        /// <summary>
        /// IO区域初始化，使用绑定
        /// </summary>
        private void initIOLed()
        {
            ledManager = new LEDManager[32];
            for (int i = 0; i < LEDCount; i++)
            {
                var grid = new Grid();
                grid.Style = (Style)FindResource("LEDGrid");

                var ellipse = new Ellipse();
                ellipse.Style = (Style)FindResource("LED");
                ledManager[i] = new LEDManager();
                var bindSetting = new Binding("EnableFlag");
                var convertor = new LEDConverter();
                bindSetting.Converter = convertor;
                bindSetting.Source = ledManager[i];
                ellipse.SetBinding(Ellipse.FillProperty, bindSetting);
                var label = new Label();
                label.Style = (Style)FindResource("LEDLable");

                grid.Children.Add(ellipse);
                grid.Children.Add(label);

                if (i < LEDCount / 2)
                {
                    label.Content = i.ToString();
                    InputLEDPanel.Children.Add(grid);
                }
                else
                {
                    var checkBox = new CheckBox();
                    checkBox.Style = (Style)FindResource("LEDCheckBox");
                    var checkBoxBinding = new Binding("EnableFlag");
                    checkBoxBinding.Source = ledManager[i];
                    checkBoxBinding.Mode = BindingMode.OneWayToSource;
                    checkBox.SetBinding(CheckBox.IsCheckedProperty, checkBoxBinding);

                    grid.Children.Add(checkBox);

                    label.Content = (i - 16).ToString();
                    OutPutLEDPanel.Children.Add(grid);
                }
            }

        }

        private void ConnetEcut(object sender, RoutedEventArgs e)
        {
            if (cutService.IsOpen())
            {
                ECutConnecter.Content = "连接e-Cut";
                infoBorad.AddInfo("已断开连接");
                timer.Tick -= AxistTick;
                timer.Tick -= LedTimerTick;
                textDisplayTimer.Tick -= textDisplayTimer_Tick;
                ControlTab.IsEnabled = false;
                ConnectLED.Fill = new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80));
                cutService.Close();
            }
            else
            {
                if (cutService.GetSumNumberOfEcut() == 0)
                {
                    infoBorad.AddInfo("没有找到e-Cut");
                    return;
                }
                try
                {
                    ConnectLED.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));

                    ControlTab.IsEnabled = true;
                    var outPutPinSettingStruct = XmlUtility.GetOutPutPinSetting();
                    var controllerConfigurationStruct = XmlUtility.GetConfig();
                    ushort dirNeg = 0;
                    for (int i = 0; i < controllerConfigurationStruct.DirInv.Length; i++)
                    {
                        if (controllerConfigurationStruct.DirInv[i])
                            dirNeg += (ushort)(1 << i);
                    }
                    cutService.Open(1);
                    cutService.StepPin = new byte[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                    cutService.DirPin = new byte[9] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
                    cutService.StepNeg = 0;
                    cutService.DirNeg = dirNeg;

                    //SmoothCoff的配置会影响最大加速度等其它参数，所以它们应该一起配置，并且SmoothCoff优于最大加速度
                    cutService.SmoothCoff = controllerConfigurationStruct.SmoothCoff;
                    cutService.StepsPerUnit = controllerConfigurationStruct.StepsPerUnit;
                    cutService.Acceleration = controllerConfigurationStruct.Acceleration;
                    cutService.MaxSpeed = controllerConfigurationStruct.MaxSpeed;
                    cutService.DelayBetweenPulseAndDir = controllerConfigurationStruct.DelayBetweenPulseAndDir;

                    cutService.eCutSetInputIOEngineDir(0, 0, new byte[64], new sbyte[64]);

                    ECutConnecter.Content = "断开连接";
                    infoBorad.AddInfo("成功连接");
                    timer.Tick += AxistTick;
                    timer.Tick += LedTimerTick;
                    textDisplayTimer.Tick += textDisplayTimer_Tick;
                    ManualAUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualXUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualYUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualZUp.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualADown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualXDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualYDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualZDown.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(ManualMouseDown), true);
                    ManualAUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualXUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualYUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualZUp.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualADown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualXDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualYDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                    ManualZDown.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(ManualMouseUp), true);
                }
                catch (Exception)
                {
                    infoBorad.OutPutNormalError();
                    return;
                }
            }
        }

        void CutPause(object sender, EventArgs e)
        {
            cutService.eCutPause();
        }

        void CutResume(object sender, EventArgs e)
        {
            cutService.eCutResume();
        }

        void textDisplayTimer_Tick(object sender, EventArgs e)
        {
            infoBorad.AddInfo("eCutActiveDepth " + cutService.eCutActiveDepth());
            infoBorad.AddInfo("eCutQueueDepth " + cutService.eCutQueueDepth());
        }

        private void ManualMouseDown(object sender, RoutedEventArgs e)
        {
            var rate = double.Parse(manualMoveRate.Percentage.Replace("%", "")) / 100.0;
            const double valueForUpMove = 99999;
            const double valueForDownMove = -99999;

            var postion = new double[9];
            ushort axis = 0;
            switch ((sender as Button).Name)
            {
                case "ManualXDown":
                    postion[0] += valueForDownMove;
                    break;
                case "ManualXUp":
                    postion[0] += valueForUpMove;
                    break;
                case "ManualYDown":
                    axis = 1;
                    postion[1] += valueForDownMove;
                    break;
                case "ManualYUp":
                    axis = 1;
                    postion[1] += valueForUpMove;
                    break;
                case "ManualZDown":
                    axis = 2;
                    postion[2] += valueForDownMove;
                    break;
                case "ManualZUp":
                    axis = 2;
                    postion[2] += valueForUpMove;
                    break;
                case "ManualADown":
                    axis = 3;
                    postion[3] += valueForDownMove;
                    break;
                case "ManualAUp":
                    axis = 3;
                    postion[3] += valueForUpMove;
                    break;
                default:
                    break;
            }
            cutService.eCutJogOn(axis, postion);
        }

        private void ManualMouseUp(object sender, RoutedEventArgs e)
        {
            cutService.EStop();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            cutService.Close();
        }

        private void ShowAxisSetting(object sender, RoutedEventArgs e)
        {
            var axisSettingWindow = new AxisSetting();
            axisSettingWindow.ShowDialog();
        }

        private void ShowOutPutPinSetting(object sender, RoutedEventArgs e)
        {
            var outPutPinSettingWindow = new OutPutPinSettingWindow();
            outPutPinSettingWindow.ShowDialog();
        }


        private void AllAxisToZero(object sender, RoutedEventArgs e)
        {
            ALLToZero();
        }

        private void ALLToZero()
        {
            var position = cutService.MachinePostion;

            for (int i = 0; i < position.Length; i++)
            {
                position[i] = -position[i];
            }
            cutService.AddLine(position, cutService.MaxSpeed.Average(), 100);
        }

        private void ConfigCoordinate(object sender, RoutedEventArgs e)
        {
            cutService.MachinePostion = (new double[9] { double.Parse(XCoordinateInput.Text), double.Parse(YCoordinateInput.Text), double.Parse(ZCoordinateInput.Text), 
            double.Parse(ACoordinateInput.Text),0, 0, 0, 0, 0});
        }

        private void LimitSet(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "文本文件|*.txt";
            var maxLimitArray = new double[4];
            var minLimitArray = new double[4];
            if (fileDialog.ShowDialog() == true)
            {
                var Text = System.IO.File.ReadAllText(fileDialog.FileName);
                var stringArray = Text.Split('%');

                if (stringArray.Length != 8)
                {
                    infoBorad.AddInfo("请输入正确的文件");
                    return;
                }
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        maxLimitArray[i] = double.Parse(stringArray[i]);
                        minLimitArray[i] = double.Parse(stringArray[4 + i]);
                    }
                }
                catch (Exception)
                {
                    infoBorad.AddInfo("请输入正确的文件");
                    return;
                }
                var result = cutService.SetSoftLimit(maxLimitArray, minLimitArray);
                infoBorad.AddInfo("配置限位成功");
            }
        }

        //Note!!! 现在G代码解析传入了当前的机械坐标
        private List<MoveInfoStruct> moveInfoList;
        /// <summary>
        /// Open the G code file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGcodeFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "文本文件|*.txt";
            if (fileDialog.ShowDialog() == true)
            {
                var Text = System.IO.File.ReadAllText(fileDialog.FileName);
                try
                {
                    moveInfoList = gCodeParser.ParseCode(Text, cutService.MachinePostion);
                }
                catch (Exception ex)
                {
                    moveInfoList = gCodeParser.ParseCode(Text, new double[9]);
                }

                if (moveInfoList == null)
                {
                    infoBorad.AddInfo("请输入正确的G代码文件");
                    return;
                }
                gCodeBorad.Clear();
                foreach (var item in moveInfoList)
                {
                    gCodeBorad.AddInfo(item.Gcode.Replace("\r", ""));
                }
                loadReady = true;
            }
        }

        private void RunGcode(object sender, RoutedEventArgs e)
        {
            foreach (var item in moveInfoList)
            {
                //TODO 速度未配置

                if (item.Type == 1)
                {
                    cutService.AddLine(new double[4]{item.Position[0],
                   item.Position[1],item.Position[2],item.Position[3]}, 5, 5);
                }
                if (item.Type == 2)
                {
                    cutService.CutAddCircle(item.CircleInfo.EndPos, item.CircleInfo.CenterPos,
                        item.CircleInfo.NormalPos, -1, 5, 5, 5);
                }
                if (item.Type == 3)
                {
                    cutService.CutAddCircle(item.CircleInfo.EndPos, item.CircleInfo.CenterPos,
                        item.CircleInfo.NormalPos, 0, 5, 5, 5);
                }

            }
        }


        #region OPENGL

        bool GlInited = false;
        double[] lastAxisVal = new double[3];
        double RotateAngle = 0;
        bool OnRotating = false;


        //更改策略：List存储导入的G代码轨迹，每次更换角度时重新导入，当前轨迹用点追踪不用线追踪，TODO：测试放大缩小
        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = GlArea.OpenGL;

            int rangeNum;
            int.TryParse(glBindNumber.Number, out rangeNum);
            //if enter wrong input, convert it to 40
            if (rangeNum == 0)
                rangeNum = 40;

            #region 绘制坐标轴
            //TODO: 增加X,Y,Z轴端点标记显示
            if (!GlInited || OnRotating)
            {
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                gl.LoadIdentity();

                //Move the geometry into a fairly central position.
                gl.Translate(0f, 0.0f, -2.0f);
                gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);

                gl.LineWidth(2);
                gl.Begin(OpenGL.GL_LINES);
                gl.Color(0f, 0f, 1.0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(0.0f, 1.0f, 0f);
                gl.Color(1f, 0f, 0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(0.0f, 0f, 1.0f);
                gl.Color(0f, 1f, 0f);
                gl.Vertex(0.0f, 0f, 0f);
                gl.Vertex(1.0f, 0f, 0f);
                gl.End();
                gl.Flush();

                GlInited = true;
            }
            #endregion
            else
            {
                gl.LoadIdentity();
                gl.Translate(0f, 0.0f, -2.0f);
                gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);

                gl.Begin(OpenGL.GL_LINES);
                gl.Color(1f, 1f, 1f);

                gl.Vertex(lastAxisVal[0] / rangeNum, lastAxisVal[1] / rangeNum, lastAxisVal[2] / rangeNum);
                gl.Vertex(AxisNumbers[0].Value / rangeNum, AxisNumbers[1].Value / rangeNum, AxisNumbers[2].Value / rangeNum);
                gl.End();
                gl.Flush();
                lastAxisVal[0] = AxisNumbers[0].Value;
                lastAxisVal[1] = AxisNumbers[1].Value;
                lastAxisVal[2] = AxisNumbers[2].Value;
            }
            if (loadReady || OnRotating)
            {
                if (moveInfoList != null)
                {
                    var array = moveInfoList.ToArray();
                    for (int i = 0; i < array.Length - 1; i++)
                    {
                        gl.LoadIdentity();
                        gl.Translate(0f, 0.0f, -2.0f);
                        gl.Rotate(RotateAngle, 1.0f, 1.0f, 1.0f);
                        if (array[i].Type == 1)
                        {
                            gl.Begin(OpenGL.GL_LINES);
                            if (array[i].Position[2] == 0 || array[i + 1].Position[2] == 0)
                                gl.Color((float)(0x68) / 255.0, (float)(0x7a) / 255.0, (float)(0xcc) / 255.0);
                            else
                                gl.Color((float)(0x255) / 255.0, (float)(0x2a) / 255.0, (float)(0x2c) / 255.0);

                            gl.Vertex(array[i].Position[0] / rangeNum, array[i].Position[1] / rangeNum, array[i].Position[2] / rangeNum);
                            gl.Vertex(array[i + 1].Position[0] / rangeNum, array[i + 1].Position[1] / rangeNum, array[i + 1].Position[2] / rangeNum);
                            gl.End();
                            gl.Flush();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重绘图像用到的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearGl(object sender, RoutedEventArgs e)
        {
            GlInited = false;
        }

        private void GlAddAngleTick(object sender, EventArgs e)
        {
            RotateAngle += 2.0;
        }

        private void GlSubAngleTick(object sender, EventArgs e)
        {
            RotateAngle -= 2.0;
        }

        #endregion

        #region 底层测试区事件处理
        private void Abort(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                if (cutService.eCutAbort())
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private bool CheckCutIsOpen()
        {
            if (!cutService.IsOpen())
            {
                infoBorad.AddInfo("没有OPEN eCut");
                return false;
            }
            return true;
        }
        private void Pause(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                if (cutService.eCutPause())
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void StopAll(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                cutService.StopAll();
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void EStop(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                cutService.EStop();
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        private void Resume(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                if (cutService.eCutResume())
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void DeviceNum(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                int result = cutService.GetSumNumberOfEcut();
                infoBorad.AddInfo("调用成功");
                DeviceNum_Result.Content = result;
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        private void eCutGetInputIO(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var result = cutService.InputIO;
                infoBorad.AddInfo("调用成功");
                if (result != null)
                    eCutGetInputIO_Result.Content = result;
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void eCutGetSteps(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var result = cutService.GetSteps();
                infoBorad.AddInfo("调用成功");
                if (result != null)
                    eCutGetSteps_Result.Content = string.Format("X:{0} Y:{1} Z:{2}", result[0], result[1], result[2]);
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void GetDeviceInfo(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var taskNumber = int.Parse(GetDeviceInfo_Number.Text);
                var charArray = new byte[12];
                if (cutService.GetDeviceInfo(taskNumber, charArray))
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
                GetDeviceInfo_Result.Content = System.Text.Encoding.GetEncoding("GB2312").GetString(charArray, 0, charArray.Length);
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        private void IOOutput(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var ushortArray = new ushort[16];
                var strArray = (IOOutput_Out.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    ushortArray[i] = ushort.Parse(strArray[i]);
                }
                cutService.eCutSetOutput(ushortArray);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void CutStop(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var taskNumber = ushort.Parse(CutStop_Out.Text);
                if (cutService.CutStop(taskNumber))
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void GetSpindlePostion(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                UInt16 result = cutService.GetSpindlePostion();
                GetSpindlePostion_Result.Content = result;
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void IsDone(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                bool result = cutService.eCutIsDone();
                IsDone_Result.Content = result;
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void GetSmoothCoff(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                UInt32 result = cutService.SmoothCoff;
                GetSmoothCoff_Result.Content = result;
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void SetSpindle(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                //TODO:WHY THOW AN EXCEPTION?
                var taskNumber = ushort.Parse(SetSpindle_Out.Text);
                cutService.setSpindle(taskNumber);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void eCutJogOn(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                UInt16 Axis = UInt16.Parse(eCutJogOn_Axis.Text);
                var doubleArray = new Double[9];
                var strArray = (eCutJogOn_PositionGiven.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    doubleArray[i] = Double.Parse(strArray[i]);
                }
                if (cutService.eCutJogOn(Axis, doubleArray))
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void eCutMoveAbsolute(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                UInt16 AxisMask = UInt16.Parse(eCutMoveAbsolute_AxisMask.Text);
                var doubleArray = new Double[9];
                var strArray = (eCutMoveAbsolute_PositionGiven.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    doubleArray[i] = Double.Parse(strArray[i]);
                }
                if (cutService.eCutMoveAbsolute(AxisMask, doubleArray))
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void SetInputIOEngineDir(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var inputIOPinArray = new Byte[64];
                var strArray = (SetInputIOEngineDir_InputIOPin.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    inputIOPinArray[i] = Byte.Parse(strArray[i]);
                }
                var EngineDirections = new SByte[64];
                strArray = (SetInputIOEngineDir_EngineDirections.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    EngineDirections[i] = SByte.Parse(strArray[i]);
                }
                var InputIOEnable = UInt64.Parse(SetInputIOEngineDir_InputIOEnable.Text);
                var InputIONeg = UInt64.Parse(SetInputIOEngineDir_InputIONeg.Text);
                var result = cutService.eCutSetInputIOEngineDir(InputIOEnable, InputIONeg, inputIOPinArray, EngineDirections);
                if (result == true)
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        private void SetSoftLimit(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var maxSoftLimitArray = new double[9];
                var strArray = (SetSoftLimit_MaxSoftLimit.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    maxSoftLimitArray[i] = double.Parse(strArray[i]);
                }
                var minSoftLimitArray = new double[9];
                strArray = (SetSoftLimit_MinSoftLimit.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    minSoftLimitArray[i] = double.Parse(strArray[i]);
                }
                var result = cutService.SetSoftLimit(maxSoftLimitArray, minSoftLimitArray);
                if (result == true)
                    infoBorad.AddInfo("调用成功");
                else
                    infoBorad.AddInfo("调用失败");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }

        //TODO:XHoming to homing
        private void XHoming(object sender, RoutedEventArgs e)
        {
            EnableUserControl(false);
            infoBorad.AddInfo("X轴回零中");
            var taskHomingPin = int.Parse(XHoming_InputIOPin.Text);

            //the UI thread shall not be block
            ThreadPool.QueueUserWorkItem(new WaitCallback(Homing), 0);
        }

        //Homing & HardLimt shall not be set in the same IO,other wise the last step of this function
        //use to correction the postion will be limit by hardlimit
        //so, if use the same pin hardlimited shall be disabled
        //TODO :Homing add pause
        /// <summary>
        /// 回原点处理
        /// </summary>
        /// <param name="homingInfo"></param>
        private void Homing(object homingInfo)
        {
            int taskHomingPin = 0;
            int inputIOVal = 0;//If the trigger way is high tri, than set it all 0, otherwise
            int axisNum = 0;
            inputIOVal = HomingWithCertainAxis(4, inputIOVal, 2);
            inputIOVal = HomingWithCertainAxis(3, inputIOVal, 1);
            inputIOVal = HomingWithCertainAxis(taskHomingPin, inputIOVal, axisNum);

            this.Dispatcher.Invoke(new Action(() =>
            {
                EnableUserControl(true);
                infoBorad.AddInfo("回零完毕");
            }));
        }

        private int HomingWithCertainAxis(int taskHomingPin, int inputIOVal, int axisNum)
        {
            var pos = cutService.MachinePostion;
            bool homingDir = false;
            //TODO : Add connect to dir in setting
            if (homingDir)
                pos[axisNum] += 999999;
            else
                pos[axisNum] -= 999999;
            cutService.eCutMoveAbsolute(15, pos);

            while ((inputIOVal & (1 << taskHomingPin)) == 0)
            {
                inputIOVal = (int)cutService.InputIO;
                Thread.Sleep(1);
            }
            cutService.EStop();
            //获取回零信号刚刚被触发时Cut所处位置,补偿
            //var eCutPosWhenHomingSignalInvoke = cutService.MachinePostion;
            //pos[axisNum] = -(cutService.MachinePostion[axisNum] - eCutPosWhenHomingSignalInvoke[axisNum]);
            //cutService.eCutMoveAbsolute(15, pos);
            pos = cutService.MachinePostion;
            if (homingDir)
                pos[axisNum] -= 999999;
            else
                pos[axisNum] += 999999;
            cutService.eCutMoveAbsolute(15, pos);
            inputIOVal = (int)cutService.InputIO;
            while ((inputIOVal & (1 << taskHomingPin)) != 0)
            {
                inputIOVal = (int)cutService.InputIO;
                Thread.Sleep(1);
            }
            cutService.EStop();
            WaitUntilCutStopMoveWithCertainAxis(axisNum);

            //回零过后使得相应轴机械坐标归0
            var eCutPos = cutService.MachinePostion;
            eCutPos[axisNum] = 0;
            cutService.MachinePostion = eCutPos;
            Thread.Sleep(500);
            return inputIOVal;
        }

        private void EnableUserControl(bool swither)
        {
            ControlTab.IsEnabled = swither;
            DisplayTab.IsEnabled = swither;
        }

        private void WaitUntilCutStopMoveWithCertainAxis(int axisNum)
        {
            double tmp = 0;
            do
            {
                tmp = cutService.MachinePostion[axisNum];
                Thread.Sleep(5);
            }
            while (cutService.MachinePostion[axisNum] != tmp);
        }

        private void SetCurrentPostion(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var pos = new double[9];
                var strArray = (SetCurrentPostion_Pos.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    pos[i] = double.Parse(strArray[i]);
                }

                var result = cutService.SetCurrentPostion(pos);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }


        private void AddCircle(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var acc = double.Parse(AddCircle_acc.Text);
                var vel = double.Parse(AddCircle_vel.Text);
                var ini_maxvel = double.Parse(AddCircle_ini_maxvel.Text);
                var turn = int.Parse(AddCircle_turn.Text);
                var pos = new double[9];
                var strArray = (AddCircle_end.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    pos[i] = double.Parse(strArray[i]);
                }
                var center = new double[9];
                strArray = (AddCircle_center.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    center[i] = double.Parse(strArray[i]);
                }
                var normal = new double[9];
                strArray = (AddCircle_normal.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    normal[i] = double.Parse(strArray[i]);
                }
                var result = cutService.CutAddCircle(pos, center, normal, turn, vel, ini_maxvel, acc);
                if (((bool)result) == true)
                    infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        private void AddLine(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var acc = double.Parse(AddLine_acc.Text);
                var vel = double.Parse(AddLine_vel.Text);
                var ini_maxvel = double.Parse(AddLine_ini_maxvel.Text);
                var pos = new double[9];
                var strArray = (AddLine_end.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    pos[i] = double.Parse(strArray[i]);
                }
                var result = cutService.CutAddLine(pos, vel, ini_maxvel, acc);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }


        private void SetStopType(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var type = byte.Parse(SetStopType_type.Text);
                var tolerance = double.Parse(SetStopType_tolerance.Text);

                bool result = cutService.SetStopType((eCutStopType)type, tolerance);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        private void MoveAtSpeed(object sender, RoutedEventArgs e)
        {
            if (!CheckCutIsOpen())
                return;
            try
            {
                var axis = ushort.Parse(MoveAtSpeed_AxisMask.Text);
                var Acceleration = new double[9];
                var strArray = (MoveAtSpeed_Acceleration.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    Acceleration[i] = double.Parse(strArray[i]);
                }
                var MaxSpeed = new double[9];
                strArray = (MoveAtSpeed_MaxSpeed.Text).Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    MaxSpeed[i] = double.Parse(strArray[i]);
                }
                var result = cutService.MoveAtSpeed(axis, Acceleration, MaxSpeed);
                infoBorad.AddInfo("调用成功");
            }
            catch (Exception)
            {
                infoBorad.AddInfo("输入了错误的参数");
            }
        }
        public bool loadReady { get; set; }
    }
        #endregion
}
