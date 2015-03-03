using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public class ControllerConfigurationStruct
    {
        public int[] StepsPerUnit { get; set; }
        public UInt32 SmoothCoff { get; set; }
        public double[] Acceleration { get; set; }
        public double[] MaxSpeed { get; set; }
        public bool[] HomingEnable { get; set; }
        public bool HomingDir { get; set; }
        public UInt16 DelayBetweenPulseAndDir { get; set; }

        public ControllerConfigurationStruct()
        {
            StepsPerUnit = new int[9];
            Acceleration = new double[9];
            MaxSpeed = new double[9];
            HomingEnable = new bool[9];
        }
    }
}
