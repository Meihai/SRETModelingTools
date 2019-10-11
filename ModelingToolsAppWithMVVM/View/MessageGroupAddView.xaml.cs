using ModelingToolsAppWithMVVM.Model;
using ModelingToolsAppWithMVVM.ViewModel;
using System.Windows;

namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Description for MessageGroupAddView.
    /// </summary>
    public partial class MessageGroupAddView : Window
    {
        /// <summary>
        /// Initializes a new instance of the MessageGroupAddView class.
        /// </summary>
        public MessageGroupAddView()
        {
            InitializeComponent();
            MessageGroup mGroup = new MessageGroup();
            this.DataContext = new MessageGroupViewModel(mGroup);          
        }

        public event delMessageGroupChanged evtMessageGroupChange;

        private void Click_Save(object sender, RoutedEventArgs e)
        {
            if (null != evtMessageGroupChange)
            {
                MessageGroupViewModel msgGroupVM=(MessageGroupViewModel) this.DataContext;
                evtMessageGroupChange(this,msgGroupVM.MessageGroup);
            }

            this.Close();
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public delegate void delMessageGroupChanged(object sender,MessageGroup mGroup);
}