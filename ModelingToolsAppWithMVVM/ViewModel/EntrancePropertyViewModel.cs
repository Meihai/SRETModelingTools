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
    public class EntrancePropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the EntrancePropertyViewModel class.
        /// </summary>
        public EntrancePropertyViewModel()
        {
            PropertyModel = new EntrancePropertyModel();
        }

        private EntrancePropertyModel propertyModel;

        public EntrancePropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }



    }
}