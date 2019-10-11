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
    /// OProcessShape.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class OProcessShape : UserControl, IOShape
    {
        public OProcessShape()
        {
            InitializeComponent();
            path.MouseDown += new MouseButtonEventHandler(path_MouseDown);
        }

        void path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnDragDrop();
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        #region IOShape 成员

        public FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.ShapeProcess; }
        }

     

        public void OnDragDrop()
        {
            DragDrop.DoDragDrop((DependencyObject)this, FlowChartType, DragDropEffects.Copy);
        }

        #endregion
    }
}
