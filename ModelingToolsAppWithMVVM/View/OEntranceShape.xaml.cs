using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.ViewModel;
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
    public partial class OEntranceShape : UserControl,IOShape,ISelectable
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
            selected = !selected;
            setBackGroundColor();
            MessageBox.Show("当前状态为："+selected);
            
        }

        private bool selected = false;

        private void setBackGroundColor() {
            if (IsSelected)
            {
                this.Background = new SolidColorBrush(Colors.AliceBlue);
            }
            else {
                this.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        #region IOShape 成员

        public  ReliabilityModelingTypes ModelType
        {
            get { return ReliabilityModelingTypes.ShapeEntrance; }
        }


        public void OnDragDrop()
        {
            DragDrop.DoDragDrop((DependencyObject)this, ModelType, DragDropEffects.Copy);
        }

        #endregion



        public bool IsSelected
        {
            get
            {
                return selected;
            }
            set
            {
                selected=value;
            }
        }
    }
}
