using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.View;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ModelingSubWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ModelingSubWindowViewModel class.
        /// </summary>
        public ModelingSubWindowViewModel()
        {
            init();
        }


        private void init()
        {
            //toolBoxPanel = new InterfaceInteractionToolView();
            toolBoxPanel = new ToolItemsBoxView();   
            profileModelingWorkPanel = new ProfileWorkView();
            ((ProfileWorkView)profileModelingWorkPanel).DataContext = new ProfileWorkViewModel();
        }
        /// <summary>
        /// 设置工具箱面板
        /// </summary>
        private object toolBoxPanel;
        public object ToolBoxPanel
        {
            get { return toolBoxPanel; }
            set
            {
                toolBoxPanel = value;
                RaisePropertyChanged(() => ToolBoxPanel);
            }
        }        

        private object profileModelingWorkPanel;

        public object ProfileModelingWorkPanel
        {
            get { return profileModelingWorkPanel; }
            set
            {
                profileModelingWorkPanel = value;
                RaisePropertyChanged(() => ProfileModelingWorkPanel);
            }
        }

        /// <summary>
        /// 信息展示区
        /// </summary>
        private object infoDisplayPanel;
        public object InfoDisplayPanel
        {
            get { return infoDisplayPanel; }
            set
            {
                infoDisplayPanel = value;
                RaisePropertyChanged(() => InfoDisplayPanel);
            }
        }

    }
}