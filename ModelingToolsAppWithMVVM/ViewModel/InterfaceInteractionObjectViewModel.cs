using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Model;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InterfaceInteractionObjectViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the InterfaceInteractionObjectViewModel class.
        /// </summary>
        public InterfaceInteractionObjectViewModel()
        {
            init();
        }

        private void init() {
            interfaceInteractionModel = new InterfaceInteractionObjectModel();
        }

        private InterfaceInteractionObjectModel interfaceInteractionModel;

        public InterfaceInteractionObjectModel InterfaceInteractionModel
        {
            get { return interfaceInteractionModel; }
            set { 
                interfaceInteractionModel = value;
                RaisePropertyChanged(() => InterfaceInteractionModel);
            }
        }


        
    }
}