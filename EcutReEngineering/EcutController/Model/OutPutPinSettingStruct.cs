using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public class OutPutPinSettingStruct
    {
        public byte[] stepPin { get; set; }
        public byte[] dirPin{get;set;}

        public OutPutPinSettingStruct()
        {
            stepPin = new byte[8];
            dirPin = new byte[8];
        }
    }
}
