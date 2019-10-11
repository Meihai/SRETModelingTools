using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
   

    /// <summary>
    ///OInterfaceInteractionObject.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class OInterfaceInteractionObject : UserControl, IOShape
    {
        public OInterfaceInteractionObject()
        {
            InitializeComponent();
            path.MouseDown += new MouseButtonEventHandler(path_MouseDown);
        }

        void path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnDragDrop();
            OnSelection();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {

            Cursor = Cursors.SizeAll;
        }


        private void Polygon_MouseEnter(object sender, MouseEventArgs e)
        {

            Cursor = Cursors.SizeAll;
        }



        #region IOShape 成员

        public FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.InterfaceInteractionObject; }
        }



        public void OnDragDrop()
        {
            DragDrop.DoDragDrop((DependencyObject)this, FlowChartType, DragDropEffects.Copy);
        }


        public void OnSelection()
        {
            Clipboard.SetData("IOShape", Enum.GetName(typeof(FlowChartTypes), FlowChartType));
        }

        #endregion
    }
}
