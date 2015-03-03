using EcutController;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Utility
{
    public class XmlUtility
    {
        //TODO: 可以简化
        public static void SetConfig(ControllerConfigurationStruct controllerConfiguration)
        {
            var element = XElement.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config")));
            var itemArray = element.Descendants("Axis").OrderBy(item => int.Parse(item.Element("Index").Value)).ToArray();

            itemArray[0].Element("StepsPerUnit").Value = controllerConfiguration.StepsPerUnit[0].ToString();
            itemArray[1].Element("StepsPerUnit").Value = controllerConfiguration.StepsPerUnit[1].ToString();
            itemArray[2].Element("StepsPerUnit").Value = controllerConfiguration.StepsPerUnit[2].ToString();
            itemArray[3].Element("StepsPerUnit").Value = controllerConfiguration.StepsPerUnit[3].ToString();

            itemArray[0].Element("MaxSpeed").Value = controllerConfiguration.MaxSpeed[0].ToString();
            itemArray[1].Element("MaxSpeed").Value = controllerConfiguration.MaxSpeed[1].ToString();
            itemArray[2].Element("MaxSpeed").Value = controllerConfiguration.MaxSpeed[2].ToString();
            itemArray[3].Element("MaxSpeed").Value = controllerConfiguration.MaxSpeed[3].ToString();

            itemArray[0].Element("Acceleration").Value = controllerConfiguration.Acceleration[0].ToString();
            itemArray[1].Element("Acceleration").Value = controllerConfiguration.Acceleration[1].ToString();
            itemArray[2].Element("Acceleration").Value = controllerConfiguration.Acceleration[2].ToString();
            itemArray[3].Element("Acceleration").Value = controllerConfiguration.Acceleration[3].ToString();

            element.Element("SmoothCoff").Value = controllerConfiguration.SmoothCoff.ToString();
            element.Element("DelayBetweenPulseAndDir").Value = controllerConfiguration.DelayBetweenPulseAndDir.ToString();

            element.Element("AxisXHomingEnable").Value = controllerConfiguration.HomingEnable[0].ToString();
            element.Element("AxisYHomingEnable").Value = controllerConfiguration.HomingEnable[1].ToString();
            element.Element("AxisZHomingEnable").Value = controllerConfiguration.HomingEnable[2].ToString();
            element.Element("AxisAHomingEnable").Value = controllerConfiguration.HomingEnable[3].ToString();
            element.Element("HomingDir").Value = controllerConfiguration.HomingDir.ToString();
            
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), element.ToString());
        }

        public static ControllerConfigurationStruct GetConfig()
        {
            var controllerConfiguration = new ControllerConfigurationStruct();

            int[] stepsPerUnit = new int[9];
            UInt32 smoothCoff;
            double[] acceleration = new double[9];
            double[] maxSpeed = new double[9];
            UInt16 delayBetweenPulseAndDir;
            var homingEnableArray = new bool[9];
            bool homingDir;
            var element = XElement.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config")));
            var itemArray = element.Descendants("Axis").OrderBy(item => int.Parse(item.Element("Index").Value)).ToArray();

            stepsPerUnit[0] = int.Parse(itemArray[0].Element("StepsPerUnit").Value);
            stepsPerUnit[1] = int.Parse(itemArray[1].Element("StepsPerUnit").Value);
            stepsPerUnit[2] = int.Parse(itemArray[2].Element("StepsPerUnit").Value);
            stepsPerUnit[3] = int.Parse(itemArray[3].Element("StepsPerUnit").Value);

            acceleration[0] = double.Parse(itemArray[0].Element("Acceleration").Value);
            acceleration[1] = double.Parse(itemArray[1].Element("Acceleration").Value);
            acceleration[2] = double.Parse(itemArray[2].Element("Acceleration").Value);
            acceleration[3] = double.Parse(itemArray[3].Element("Acceleration").Value);

            maxSpeed[0] = double.Parse(itemArray[0].Element("MaxSpeed").Value);
            maxSpeed[1] = double.Parse(itemArray[1].Element("MaxSpeed").Value);
            maxSpeed[2] = double.Parse(itemArray[2].Element("MaxSpeed").Value);
            maxSpeed[3] = double.Parse(itemArray[3].Element("MaxSpeed").Value);

            homingEnableArray[0] = bool.Parse(element.Element("AxisXHomingEnable").Value);
            homingEnableArray[1] = bool.Parse(element.Element("AxisYHomingEnable").Value);
            homingEnableArray[2] = bool.Parse(element.Element("AxisZHomingEnable").Value);
            homingEnableArray[3] = bool.Parse(element.Element("AxisAHomingEnable").Value);

            homingDir = bool.Parse(element.Element("HomingDir").Value);

            smoothCoff = UInt32.Parse(element.Element("SmoothCoff").Value);
            delayBetweenPulseAndDir = UInt16.Parse(element.Element("DelayBetweenPulseAndDir").Value);

            controllerConfiguration.Acceleration = acceleration;
            controllerConfiguration.MaxSpeed = maxSpeed;
            controllerConfiguration.DelayBetweenPulseAndDir = delayBetweenPulseAndDir;
            controllerConfiguration.SmoothCoff = smoothCoff;
            controllerConfiguration.StepsPerUnit = stepsPerUnit;
            controllerConfiguration.HomingEnable = homingEnableArray;
            controllerConfiguration.HomingDir = homingDir;


            return controllerConfiguration;
        }

        public static OutPutPinSettingStruct GetOutPutPinSetting()
        {
            var outPutPinSetting = new OutPutPinSettingStruct();

            var element = XElement.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config")));
            outPutPinSetting.stepPin[0] = byte.Parse(element.Element("XSTepPinSet").Value);
            outPutPinSetting.stepPin[1] = byte.Parse(element.Element("YSTepPinSet").Value);
            outPutPinSetting.stepPin[2] = byte.Parse(element.Element("ZSTepPinSet").Value);
            outPutPinSetting.stepPin[3] = byte.Parse(element.Element("ASTepPinSet").Value);
            outPutPinSetting.dirPin[0] = byte.Parse(element.Element("XDirPinSet").Value);
            outPutPinSetting.dirPin[1] = byte.Parse(element.Element("YDirPinSet").Value);
            outPutPinSetting.dirPin[2] = byte.Parse(element.Element("ZDirPinSet").Value);
            outPutPinSetting.dirPin[3] = byte.Parse(element.Element("ADirPinSet").Value);

            return outPutPinSetting;
        }

        public static void SetOutPutPinSetting(OutPutPinSettingStruct outPutPinSetting)
        {
            var element = XElement.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config")));

            element.Element("XSTepPinSet").Value = outPutPinSetting.stepPin[0].ToString();
            element.Element("YSTepPinSet").Value = outPutPinSetting.stepPin[1].ToString();
            element.Element("ZSTepPinSet").Value = outPutPinSetting.stepPin[2].ToString();
            element.Element("ASTepPinSet").Value = outPutPinSetting.stepPin[3].ToString();
            element.Element("XDirPinSet").Value = outPutPinSetting.dirPin[0].ToString();
            element.Element("YDirPinSet").Value = outPutPinSetting.dirPin[1].ToString();
            element.Element("ZDirPinSet").Value = outPutPinSetting.dirPin[2].ToString();
            element.Element("ADirPinSet").Value = outPutPinSetting.dirPin[3].ToString();

            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), element.ToString());
        }
    }
}
