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
using ModelingToolsApp.UserControls;

namespace ModelingToolsApp
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


        private void MenuItem_Click_NewFile(object sender, RoutedEventArgs e)
        {
            this.viewer.MenuItem_Click_NewFile(sender, e);
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewer.Delete_Executed(sender, e);
        }


        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            this.viewer.Delete_Enabled(sender, e);
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewer.Copy_Executed(sender, e);
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            this.viewer.Copy_Enabled(sender, e);
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewer.Paste_Executed(sender, e);
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            this.viewer.Paste_Enabled(sender, e);

        }

        private void MenuItem_Click_ColorSelection(object sender, RoutedEventArgs e)
        {
            this.viewer.MenuItem_Click_ColorSelection(sender, e);
        }
        
    }
}
