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
    /// OEntranceShape.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class OEntranceShape : UserControl, IOShape
    {
        public OEntranceShape()
        {
            InitializeComponent();
            path.MouseDown += new MouseButtonEventHandler(path_MouseDown);
           
        }

        void path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnDragDrop();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }


        private void Ellipse_MouseLeftButtonDown(object sender,MouseEventArgs e)
        {
           
            //MessageBox.Show("鼠标左键按下");
            
        }


        #region IOShape 成员

        public FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.ShapeEntrance; }
        }





        public void OnDragDrop()
        {
            DragDrop.DoDragDrop((DependencyObject)this, FlowChartType, DragDropEffects.Copy);
        }

        #endregion
    }
}
