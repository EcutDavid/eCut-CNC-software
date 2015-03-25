using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    public class CutUtility
    {
        /// <summary>
        /// 获取与PC连接的板卡数量
        /// </summary>
        /// <returns>与PC连接的板卡数量</returns>
        public static int GetConnectedCutNum()
        {
            return eCutDevice.GetDeviceNum();
        }

        /// <summary>
        /// 获取板卡序列号
        /// </summary>
        /// <param name="cutIndex"></param>
        /// <returns>序列号</returns>
        public static String GetCutSerialNumber(int num)
        {
            if ((CutUtility.GetConnectedCutNum() - 1) < num)
                throw new CutNotExistException();
            var charArray = new byte[12];
            eCutDevice.GetDeviceInfo(num, charArray);
            return Encoding.GetEncoding("GB2312").GetString(charArray, 0, charArray.Length).ToString();
        }

        public static IEcutService GetCutService(int cutIndex)
        {
            return new EcutEntity(cutIndex);
        }
    }
}
