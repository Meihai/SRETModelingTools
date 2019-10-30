using CommonServiceLocator;
using ModelingToolsAppWithMVVM.Common.ChartCommon;
using ModelingToolsAppWithMVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Description for InterfaceInteractionModelingView.
    /// </summary>
    public partial class InterfaceInteractionModelingView : Window
    {
        /// <summary>
        /// Initializes a new instance of the InterfaceInteractionModelingView class.
        /// </summary>
        public InterfaceInteractionModelingView()
        {
            InitializeComponent();
            initCommandBindings();
        }

        private void initCommandBindings()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
           // this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
           // this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste,Paste_Executed,Paste_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.Source is TabControl)
            //{
            //    workBox = new InterfaceInteractionToolView();
            //    workBox.DataContext = ServiceLocator.Current.GetInstance<InterfaceInteractionWorkViewModel>();

            //    profileBox = new InterfaceInteractionToolView();
            //    profileBox.DataContext = ServiceLocator.Current.GetInstance<InterfaceInteractionWorkViewModel>();
                
            //}
        }

        #region 删除模型
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //两个不同的界面，剖面建模界面和接口建模界面
            if(this.tabControl.SelectedIndex==0 && this.workBox.Content is InterfaceInteractionWorkView){
                ((InterfaceInteractionWorkModel)(((InterfaceInteractionWorkView)(this.workBox.Content)).workPanel.Content)).Delete_Executed(sender, e);
            }else if(this.tabControl.SelectedIndex==1 && this.profileBox.Content is ProfileWorkView){
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Delete_Executed(sender, e);
            }            
           
        }
        
        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e){
            //两个不同的界面，剖面建模界面和接口建模界面    
            if(this.tabControl.SelectedIndex==0 && this.workBox.Content is InterfaceInteractionWorkView) {
                ((InterfaceInteractionWorkModel)(((InterfaceInteractionWorkView)(this.workBox.Content)).workPanel.Content)).Delete_Enabled(sender, e);
            } 
            else if (this.tabControl.SelectedIndex==1 && this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Delete_Enabled(sender, e);
            }
           
        }
        #endregion 删除模型

        #region 打开文件
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.tabControl.SelectedIndex == 0 && this.workBox.Content is InterfaceInteractionWorkView)
            {
                ((InterfaceInteractionWorkModel)(((InterfaceInteractionWorkView)(this.workBox.Content)).workPanel.Content)).Open_Executed(sender, e);
               
            }
            else if (this.tabControl.SelectedIndex == 1 && this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Open_Executed(sender, e);              
            }
        }
        #endregion 打开文件

        #region 保存文件
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.tabControl.SelectedIndex == 0 && this.workBox.Content is InterfaceInteractionWorkView)
            {
                ((InterfaceInteractionWorkModel)(((InterfaceInteractionWorkView)(this.workBox.Content)).workPanel.Content)).Save_Executed(sender, e);

            }
            else if (this.tabControl.SelectedIndex == 1 && this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).Save_Executed(sender, e);
            }
        }
        #endregion 保存文件

        #region 新建模型文件
        private void New_Executed(object sender,ExecutedRoutedEventArgs e){
         
            if (this.tabControl.SelectedIndex == 0 && this.workBox.Content is InterfaceInteractionWorkView)
            {
                ((InterfaceInteractionWorkModel)(((InterfaceInteractionWorkView)(this.workBox.Content)).workPanel.Content)).New_Executed(sender, e);
               
            }
            else if (this.tabControl.SelectedIndex == 1 && this.profileBox.Content is ProfileWorkView)
            {
                ((ProfileWorkModel)(((ProfileWorkView)(this.profileBox.Content)).workPanel.Content)).New_Executed(sender, e);
            }
        }

        #endregion 新建模型文件
    }
}