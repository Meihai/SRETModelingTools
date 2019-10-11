using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.View;
using System.Threading;
using System.Windows.Threading;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InterfaceInteractionModelingViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the InterfaceInteractionModelingViewModel class.
        /// </summary>
        public InterfaceInteractionModelingViewModel()
        {
            init();
        }


        private void init()
        {
            //toolBoxPanel = new InterfaceInteractionToolView();
            toolBoxPanel = new ToolItemsBoxView();

            modelingWorkPanel = new InterfaceInteractionWorkView();           

            profileModelingWorkPanel = new ProfileWorkView();
            
        }
        /// <summary>
        /// 设置工具箱面板
        /// </summary>
        private object toolBoxPanel;

        public object ToolBoxPanel
        {
            get { return toolBoxPanel; }
            set {
                toolBoxPanel = value;
                RaisePropertyChanged(() => ToolBoxPanel);
            }
        }

        /// <summary>
        /// 设置软件交联建模工作区面板
        /// </summary>
        private object modelingWorkPanel;

        public object ModelingWorkPanel
        {
            get { return modelingWorkPanel; }
            set { 
                modelingWorkPanel = value;
                RaisePropertyChanged(() => ModelingWorkPanel);
            }
        }


        private object profileModelingWorkPanel;

        public object ProfileModelingWorkPanel
        {
            get { return profileModelingWorkPanel; }
            set { 
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
            set { 
                infoDisplayPanel = value;
                RaisePropertyChanged(() => InfoDisplayPanel);
            }
        }




    }
}