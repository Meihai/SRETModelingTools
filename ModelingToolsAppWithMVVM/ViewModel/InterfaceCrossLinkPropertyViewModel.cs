using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using ModelingToolsAppWithMVVM.View;
using System.Windows;
using CommonServiceLocator;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InterfaceCrossLinkPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the InterfaceCrossLinkPropertyViewModel class.
        /// </summary>
        public InterfaceCrossLinkPropertyViewModel()
        {
            crossLinkProperty = new InterfaceCrossLinkPropertyModel();
        }

        private InterfaceCrossLinkPropertyModel crossLinkProperty;

        public InterfaceCrossLinkPropertyModel CrossLinkProperty
        {
            get { return crossLinkProperty; }
            set { crossLinkProperty = value;
                  RaisePropertyChanged(() => CrossLinkProperty);
            }
        }


        private ICommand gotoInterfacePropertyViewCommand;

        public ICommand GotoInterfacePropertyViewCommand
        {
            get
            {
                return gotoInterfacePropertyViewCommand ?? (gotoInterfacePropertyViewCommand = new RelayCommand(

                       GotoInterfacePropertyView, () => { return true; }
                   ));
            }
        }

        private void GotoInterfacePropertyView()
        {
            //InterfacePropertyView interfacePropertyWindow = new InterfacePropertyView();      
            InterfaceConfigDataGridView interfacePropertyWindow = new InterfaceConfigDataGridView();
            InterfacePropertyViewModel dataContext1 = ServiceLocator.Current.GetInstance<InterfacePropertyViewModel>();
            interfacePropertyWindow.DataContext = dataContext1;
            interfacePropertyWindow.ShowDialog();
        }
   



    }
}