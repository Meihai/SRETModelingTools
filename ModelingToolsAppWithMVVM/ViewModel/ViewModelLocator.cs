/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ModelingToolsAppWithMVVM"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
                     

            SimpleIoc.Default.Register<InterfacePropertyViewModel>();
            SimpleIoc.Default.Register<MessageManagerViewModel>();
            SimpleIoc.Default.Register<MessageGroupViewModel>();
            SimpleIoc.Default.Register<MessageViewModel>();      
            SimpleIoc.Default.Register<InterfaceInteractionModelingViewModel>();
            SimpleIoc.Default.Register<InterfaceInteractionWorkViewModel>();
            SimpleIoc.Default.Register<InterfaceCrossLinkPropertyViewModel>();
            SimpleIoc.Default.Register<InterfaceInteractionObjectViewModel>();
            SimpleIoc.Default.Register<InterfaceInteractionTestedObjectPropertyViewModel>();
            SimpleIoc.Default.Register<ProfileWorkViewModel>();
        }

        #region สตภýปฏ
            
        public InterfacePropertyViewModel InterfacePropertyView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InterfacePropertyViewModel>();
            }
        }

        public MessageManagerViewModel MessageManagerView
        {
            get {
                return ServiceLocator.Current.GetInstance<MessageManagerViewModel>();
            }
        }


        public MessageGroupViewModel MessageGroupView
        {
            get { 
                return ServiceLocator.Current.GetInstance<MessageGroupViewModel>();
            }
        }

        public MessageViewModel MessageView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MessageViewModel>();
            }
        }


        public InterfaceInteractionModelingViewModel InterfaceInteractionModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InterfaceInteractionModelingViewModel>();
            }
        }
     

        public InterfaceInteractionWorkViewModel InterfaceInteractionWorkView{      
            get
            {
                return ServiceLocator.Current.GetInstance<InterfaceInteractionWorkViewModel>();
            }
        }

        public InterfaceCrossLinkPropertyViewModel InterfaceCrossLinkPropertyView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InterfaceCrossLinkPropertyViewModel>();
            }
        }


        public InterfaceInteractionObjectViewModel InterfaceInteractionObjectView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InterfaceInteractionObjectViewModel>();
            }
        }

        public InterfaceInteractionTestedObjectPropertyViewModel IITestedObjectPropertyView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InterfaceInteractionTestedObjectPropertyViewModel>();
            }
        }

        public ProfileWorkViewModel ProfileWorkView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProfileWorkViewModel>();
            }
        }

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}