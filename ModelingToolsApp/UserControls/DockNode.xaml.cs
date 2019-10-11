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

namespace ModelingToolsApp.UserControls
{
    /// <summary>
    /// LinkNode.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class DockNode : UserControl
    {
        public DockNode()
        {
            InitializeComponent();

            GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
        }

        void GlobalMouseHook_evtGlobalMouseUp(GlobalMouseArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        public void AccepteMouseMove(MouseEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                Point p = e.GetPosition(this);
                if (p.X < 0 || p.X > Width || p.Y < 0 || p.Y > Height)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    Cursor = Cursors.Cross;
                }
            }
        }
    }
}
