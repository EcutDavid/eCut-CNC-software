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
using Utility;


namespace P1S1
{
    /// <summary>
    /// Interaction logic for OutputPinSetting.xaml
    /// </summary>
    public partial class OutPutPinSettingWindow : Window
    {
        public OutPutPinSettingWindow()
        {
            InitializeComponent();
            var outPutPinSetting = XmlUtility.GetOutPutPinSetting();
            XStepCB.SelectedIndex = outPutPinSetting.stepPin[0];
            YStepCB.SelectedIndex = outPutPinSetting.stepPin[1];
            ZStepCB.SelectedIndex = outPutPinSetting.stepPin[2];
            AStepCB.SelectedIndex = outPutPinSetting.stepPin[3];

            XDirCB.SelectedIndex = outPutPinSetting.dirPin[0] - 8;
            YDirCB.SelectedIndex = outPutPinSetting.dirPin[1] - 8;
            ZDirCB.SelectedIndex = outPutPinSetting.dirPin[2] - 8;
            ADirCB.SelectedIndex = outPutPinSetting.dirPin[3] - 8;

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        byte[] stepConst = { 0, 1, 2, 3};
        byte[] dirConst = { 8, 9, 10, 11 };
        
        private void ConformClick(object sender, RoutedEventArgs e)
        {
            var outPutPinSetting = new OutPutPinSettingStruct();
            outPutPinSetting.stepPin[0] = stepConst[XStepCB.SelectedIndex];
            outPutPinSetting.stepPin[1] = stepConst[YStepCB.SelectedIndex];
            outPutPinSetting.stepPin[2] = stepConst[ZStepCB.SelectedIndex];
            outPutPinSetting.stepPin[3] = stepConst[AStepCB.SelectedIndex];

            outPutPinSetting.dirPin[0] = dirConst[XDirCB.SelectedIndex];
            outPutPinSetting.dirPin[1] = dirConst[YDirCB.SelectedIndex];
            outPutPinSetting.dirPin[2] = dirConst[ZDirCB.SelectedIndex];
            outPutPinSetting.dirPin[3] = dirConst[ADirCB.SelectedIndex];

            XmlUtility.SetOutPutPinSetting(outPutPinSetting);
            this.Close();
        }
    }
}
