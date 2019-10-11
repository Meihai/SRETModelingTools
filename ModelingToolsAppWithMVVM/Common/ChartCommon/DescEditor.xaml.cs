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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public delegate void delDescriptionChanged(string desc);

    /// <summary>
    /// DescEditor.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class DescEditor : UserControl
    {
        private bool IsAutoWidth = false;

        private Point OriginalPosi = new Point(0, 0);

        private IFlowChartBase ifcb;

        public DescEditor()
        {
            InitializeComponent();
            
        }

        public new void Focus()
        {
            this.txtEditor.Focus();
            this.txtEditor.SelectAll();
        }

        public void BindFlowChart(IFlowChartBase ifcb)
        {
            bool res = ifcb.GetDescriptionPosition(out OriginalPosi);
            IsAutoWidth = res;

            this.ifcb = ifcb;
            this.txtEditor.Text = ifcb.Description;

            if (!res) //shape
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
                this.VerticalAlignment = VerticalAlignment.Center;
                this.Margin = ifcb.Margin;
                this.Width = ifcb.Width;
                this.Height = ifcb.Height;
            }
            else  //link
            {
                this.txtEditor.FontSize = 14;
                this.HorizontalAlignment = HorizontalAlignment.Left;
                this.VerticalAlignment = VerticalAlignment.Top;

                double width = 0;
                double height = 0;
                CalcDescTextSize(out width, out height);

                this.Margin = new Thickness(OriginalPosi.X - width / 4, OriginalPosi.Y - height, 0, 0);
                this.Height = 35;
            }
        }

        private void txtEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render,
              new Action(() =>
              {
                  txtEditor.Focus();
              }));
        }

        private void txtEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsAutoWidth)
            {
                double width = 0;
                double height = 0;
                CalcDescTextSize(out width, out height);

                this.Width = width / 2;
                this.Height = 35;
                this.Margin = new Thickness(OriginalPosi.X - width / 4, OriginalPosi.Y - height, 0, 0);

            }

            if (null != this.ifcb)
            {
                this.ifcb.Description = txtEditor.Text;
                
                
            }
        }


        /// <summary>
        /// 计算说明文字的所占的尺寸(像素)
        /// </summary>
        /// <returns></returns>
        protected void CalcDescTextSize(out double width, out double height)
        {
            width = 0;
            height = 0;
            //区分汉字和英文字母，统计宽度
            for (int i = 0; i < this.txtEditor.Text.Length; i++)
            {
                if (this.txtEditor.Text[i] <= 127)
                {
                    width += txtEditor.FontSize;
                }
                else
                {
                    width += txtEditor.FontSize * 2;
                }
            }
            width += txtEditor.FontSize * 4; //多算一字符
            height = txtEditor.FontSize;
            return;
        }
    }
}
