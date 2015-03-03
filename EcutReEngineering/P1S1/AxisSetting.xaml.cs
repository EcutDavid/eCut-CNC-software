using EcutController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Utility;


namespace P1S1
{
    /// <summary>
    /// Interaction logic for AxisSetting.xaml
    /// </summary>
    public partial class AxisSetting : Window
    {
        public AxisSetting()
        {
            InitializeComponent();
            var configration = XmlUtility.GetConfig();
            
            XStepPerUnitBox.Text = configration.StepsPerUnit[0].ToString();
            YStepPerUnitBox.Text = configration.StepsPerUnit[1].ToString();
            ZStepPerUnitBox.Text = configration.StepsPerUnit[2].ToString();
            AStepPerUnitBox.Text = configration.StepsPerUnit[3].ToString();

            XMaxSppedBox.Text = configration.MaxSpeed[0].ToString();
            YMaxSppedBox.Text = configration.MaxSpeed[1].ToString();
            ZMaxSppedBox.Text = configration.MaxSpeed[2].ToString();
            AMaxSppedBox.Text = configration.MaxSpeed[3].ToString();

            XMaxAccBox.Text = configration.Acceleration[0].ToString();
            YMaxAccBox.Text = configration.Acceleration[1].ToString();
            ZMaxAccBox.Text = configration.Acceleration[2].ToString();
            AMaxAccBox.Text = configration.Acceleration[3].ToString();

            AxisXHomingCheckBox.IsChecked = configration.HomingEnable[0];
            AxisYHomingCheckBox.IsChecked = configration.HomingEnable[1];
            AxisZHomingCheckBox.IsChecked = configration.HomingEnable[2];
            AxisAHomingCheckBox.IsChecked = configration.HomingEnable[3];

            if (configration.HomingDir)
            {
                HomingDirComboBox.SelectedIndex = 0;
            }
            else
            {
                HomingDirComboBox.SelectedIndex = 1;
            }

            PulseDirDelayBox.Text = configration.DelayBetweenPulseAndDir.ToString();
            SmoothComboBox.SelectedIndex = (int)Math.Log(configration.SmoothCoff, 2);
           
        }
        
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConformClick(object sender, RoutedEventArgs e)
        {
            var controllerConfiguration = new ControllerConfigurationStruct();
            controllerConfiguration.StepsPerUnit[0] = int.Parse(XStepPerUnitBox.Text);
            controllerConfiguration.StepsPerUnit[1] = int.Parse(YStepPerUnitBox.Text);
            controllerConfiguration.StepsPerUnit[2] = int.Parse(ZStepPerUnitBox.Text);
            controllerConfiguration.StepsPerUnit[3] = int.Parse(AStepPerUnitBox.Text);

            controllerConfiguration.MaxSpeed[0] = double.Parse(XMaxSppedBox.Text);
            controllerConfiguration.MaxSpeed[1] = double.Parse(YMaxSppedBox.Text);
            controllerConfiguration.MaxSpeed[2] = double.Parse(ZMaxSppedBox.Text);
            controllerConfiguration.MaxSpeed[3] = double.Parse(AMaxSppedBox.Text);

            controllerConfiguration.Acceleration[0] = double.Parse(XMaxAccBox.Text);
            controllerConfiguration.Acceleration[1] = double.Parse(YMaxAccBox.Text);
            controllerConfiguration.Acceleration[2] = double.Parse(ZMaxAccBox.Text);
            controllerConfiguration.Acceleration[3] = double.Parse(AMaxAccBox.Text);

            controllerConfiguration.DelayBetweenPulseAndDir = UInt16.Parse(PulseDirDelayBox.Text);
            controllerConfiguration.SmoothCoff = (uint)(1 << SmoothComboBox.SelectedIndex) ;

            controllerConfiguration.HomingEnable[0] = (bool)AxisXHomingCheckBox.IsChecked;
            controllerConfiguration.HomingEnable[1] = (bool)AxisYHomingCheckBox.IsChecked;
            controllerConfiguration.HomingEnable[2] = (bool)AxisZHomingCheckBox.IsChecked;
            controllerConfiguration.HomingEnable[3] = (bool)AxisAHomingCheckBox.IsChecked;
            
            if (HomingDirComboBox.SelectedIndex == 0)
            {
                controllerConfiguration.HomingDir = true;
            }
            else
            {
                controllerConfiguration.HomingDir = false;
            }
            XmlUtility.SetConfig(controllerConfiguration);
            this.Close();
        }
    }
}
