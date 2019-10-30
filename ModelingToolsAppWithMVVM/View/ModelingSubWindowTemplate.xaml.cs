using ModelingToolsAppWithMVVM.Common.ChartCommon;
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
using System.Windows.Shapes;


namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Interaction logic for ModelingSubWindowTemplate.xaml
    /// </summary>
    public partial class ModelingSubWindowTemplate : Window
    {
        public ModelingSubWindowTemplate()
        {
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        

        #region 删除模型
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Delete_Executed(sender, e);
            }

        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Delete_Enabled(sender, e);
            }

        }
        #endregion 删除模型

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
           
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click_ColorSelection(object sender, RoutedEventArgs e)
        {
           
        }
                    
        
    }
}
