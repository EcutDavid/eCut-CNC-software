using EcutController;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utility
{
    public class gCodeParser
    {
        /// <summary>
        /// 解析代码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos">当前机械坐标</param>
        /// <returns></returns>
        public static List<MoveInfoStruct> ParseCode(string text, double[] pos)
        {
            //运动信息列表，为NULL则解析错误
            var moveInfoList = new List<MoveInfoStruct>();
            text = Regex.Replace(text, " ", "").ToUpper();

            //去掉N92等信息
            text = Regex.Replace(text, "(N[0-9]*)|(\r)", "");
            var gCodeList = text.Split('\n').ToList();
            //去掉文本中为空的行
            gCodeList.RemoveAll(item => item == "");

            //判断G代码是否为有效G代码
            foreach (var item in gCodeList)
            {
                if (!IsGCodeValid(item))
                    return null;
            }

            //逐条解析G代码
            foreach (var item in gCodeList)
            {
                moveInfoList.Add(parseSingleOrder(item, pos));
            }
            return moveInfoList;
        }

        /// <summary>
        /// 解析单条指令
        /// </summary>
        /// <param name="singleOrder"></param>
        /// <returns></returns>
        private static MoveInfoStruct parseSingleOrder(string singleOrder, double[] pos)
        {
            var moveInfoStruct = new MoveInfoStruct();
            moveInfoStruct.Gcode = singleOrder;
            if (singleOrder.Contains('G'))
            {
                GetMoveType(singleOrder, moveInfoStruct);

                if (moveInfoStruct.Type == 1)
                    GetLineInfo(singleOrder, moveInfoStruct, pos);
                if (moveInfoStruct.Type == 2 || moveInfoStruct.Type == 3)
                    GetCircleInfo(singleOrder, moveInfoStruct, pos);
            }

            return moveInfoStruct;
        }


        /// <summary>
        /// 获取运动类型
        /// </summary>
        /// <param name="singleOrder"></param>
        /// <param name="moveInfoStruct"></param>
        private static void GetMoveType(string singleOrder, MoveInfoStruct moveInfoStruct)
        {
            var remainText = Regex.Split(singleOrder, "G")[1];
            var moveInfoDataItem = Regex.Split(remainText, "[X-Z]")[0];
            switch (int.Parse(moveInfoDataItem))
            {
                case 0:
                case 1:
                    moveInfoStruct.Type = 1;
                    break;
                case 2:
                    moveInfoStruct.Type = 2;
                    break;
                case 3:
                    moveInfoStruct.Type = 3;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 解析G02 03用
        /// </summary>
        /// <param name="singleOrder"></param>
        /// <param name="moveInfoStruct"></param>
        /// <param name="pos"></param>
        private static void GetCircleInfo(string singleOrder, MoveInfoStruct moveInfoStruct, double[] pos)
        {
            if (singleOrder.Contains('X') && singleOrder.Contains('Y') && singleOrder.Contains('I') && singleOrder.Contains('J'))
            {
                moveInfoStruct.CircleInfo.NormalPos = new double[3] { 0, 0, 1 };
                var remainText = Regex.Split(singleOrder, "X")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.CircleInfo.EndPos[0] = double.Parse(moveInfoDataItem);
                remainText = Regex.Split(singleOrder, "Y")[1];
                moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.CircleInfo.EndPos[1] = double.Parse(moveInfoDataItem);
                remainText = Regex.Split(singleOrder, "I")[1];
                moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.CircleInfo.CenterPos[0] = pos[0] + double.Parse(moveInfoDataItem);
                remainText = Regex.Split(singleOrder, "J")[1];
                moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.CircleInfo.CenterPos[1] = pos[1] + double.Parse(moveInfoDataItem);
                moveInfoStruct.CircleInfo.CenterPos[2] = moveInfoStruct.CircleInfo.EndPos[2] = pos[2];
                pos[0] = moveInfoStruct.CircleInfo.EndPos[0];
                pos[1] = moveInfoStruct.CircleInfo.EndPos[1];
            }

            if (singleOrder.Contains('F'))
            {
                var remainText = Regex.Split(singleOrder, "F")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.Speed = double.Parse(moveInfoDataItem);
            }
        }

        /// <summary>
        /// 解析G00 01用
        /// </summary>
        /// <param name="singleOrder"></param>
        /// <param name="moveInfoStruct"></param>
        /// <param name="pos"></param>
        private static void GetLineInfo(string singleOrder, MoveInfoStruct moveInfoStruct, double[] pos)
        {
            if (singleOrder.Contains('X'))
            {
                var remainText = Regex.Split(singleOrder, "X")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                pos[0] = moveInfoStruct.Position[0] = double.Parse(moveInfoDataItem);
            }
            else
                moveInfoStruct.Position[0] = pos[0];

            if (singleOrder.Contains('Y'))
            {
                var remainText = Regex.Split(singleOrder, "Y")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                pos[1] = moveInfoStruct.Position[1] = double.Parse(moveInfoDataItem);
            }
            else
                moveInfoStruct.Position[1] = pos[1];

            if (singleOrder.Contains('Z'))
            {
                var remainText = Regex.Split(singleOrder, "Z")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                pos[2] = moveInfoStruct.Position[2] = double.Parse(moveInfoDataItem);
            }
            else
                moveInfoStruct.Position[2] = pos[2];

            if (singleOrder.Contains('A'))
            {
                var remainText = Regex.Split(singleOrder, "A")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                pos[3] = moveInfoStruct.Position[3] = double.Parse(moveInfoDataItem);
            }
            else
                moveInfoStruct.Position[3] = pos[3];

            if (singleOrder.Contains('F'))
            {
                var remainText = Regex.Split(singleOrder, "F")[1];
                var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F|A")[0];
                moveInfoStruct.Speed = double.Parse(moveInfoDataItem);
            }
        }

        /// <summary>
        /// 判断是否为有效的G代码
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        private static bool IsGCodeValid(string Code)
        {
            return Regex.IsMatch(Code, "(^G0?[0-3]([XYZIJKRF]-?[0-9]+.?[0-9]*)+?)$");
        }
    }
}
