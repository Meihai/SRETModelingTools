using ModelingToolsAppWithMVVM.ViewModel;
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

namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Interaction logic for MessageManagerUserControlView.xaml
    /// </summary>
    public partial class MessageManagerUserControlView : UserControl
    {
        public MessageManagerUserControlView()
        {
            InitializeComponent();
            this.DataContext = new MessageManagerViewModel();
            ((MessageManagerViewModel)this.DataContext).evtRefreshDataContext += evtRefreshDataContext;
        }

        private void evtRefreshDataContext()
        {
            var dataContext = this.DataContext;
            this.DataContext = null;
            this.DataContext = dataContext;
        }
    }
}
