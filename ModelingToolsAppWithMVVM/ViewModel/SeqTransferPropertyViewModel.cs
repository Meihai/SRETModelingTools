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
    public class SeqTransferPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SeqTransferPropertyViewModel class.
        /// </summary>
        public SeqTransferPropertyViewModel()
        {
            propertyModel = new SeqTransferPropertyModel();
        }

        private SeqTransferPropertyModel propertyModel;

        public SeqTransferPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }

    }
}