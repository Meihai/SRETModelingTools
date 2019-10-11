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
    public class ProbTransferPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ProbTransferPropertyViewModel class.
        /// </summary>
        public ProbTransferPropertyViewModel()
        {
            propertyModel = new ProbTransferPropertyModel();
        }

        private ProbTransferPropertyModel propertyModel;

        public ProbTransferPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }



    }
}