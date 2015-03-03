using System.Windows.Controls;
using System.Windows.Media;

namespace Utility
{
    public class InfoBorad
    {
        private ListBox listBox;

        public InfoBorad(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public void Clear()
        {
            this.listBox.Items.Clear();
        }

        public void SelectItem(int index)
        {
            this.listBox.SelectedIndex = index;
        }

        public void AddInfo(string targetInfo)
        {
            this.listBox.Items.Add(targetInfo);

            var border = (Border)VisualTreeHelper.GetChild(this.listBox, 0);
            var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            scrollViewer.ScrollToBottom();
        }

        public void OutPutNormalError()
        {
            AddInfo("软件异常，请与技术人员练习");
        }
    }
}
