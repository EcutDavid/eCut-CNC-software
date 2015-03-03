using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public class CircleEntity
    {
        public double[] EndPos { get; set; }
        public double[] CenterPos { get; set; }
        public double[] NormalPos { get; set; }
        public int PlaneType { get; set; }

        public CircleEntity()
        {
            EndPos = new double[4];
            CenterPos = new double[4];
            NormalPos = new double[4];
        }
    }

    public class MoveInfoStruct
    {
        public int Type { get; set; }
        public double[] Position { get; set; }
        public double Speed { get; set; }
        public string Gcode { get; set; }
        public CircleEntity CircleInfo { get; set; }
        public MoveInfoStruct()
        {
            Position = new double[4];
            CircleInfo = new CircleEntity();
        }
    }
}
