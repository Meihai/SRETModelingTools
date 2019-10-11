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
    public class ReliableProfilePropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ReliableProfilePropertyViewModel class.
        /// </summary>
        public ReliableProfilePropertyViewModel()
        {
            propertyModel = new ReliableProfilePropertyModel();
        }

        private ReliableProfilePropertyModel propertyModel;

        public ReliableProfilePropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }

    }
}