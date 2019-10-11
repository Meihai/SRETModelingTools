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
    public class ExitPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ExitViewModel class.
        /// </summary>
        public ExitPropertyViewModel()
        {
            propertyModel = new ExitPropertyModel();
        }

        private ExitPropertyModel propertyModel;

        public ExitPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }

    }
}