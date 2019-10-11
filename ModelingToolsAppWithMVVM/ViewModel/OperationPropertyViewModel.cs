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
    public class OperationPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the OperationPropertyViewModel class.
        /// </summary>
        public OperationPropertyViewModel()
        {
            propertyModel = new OperationPropertyModel();
        }

        private OperationPropertyModel propertyModel;

        public OperationPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }




    }
}