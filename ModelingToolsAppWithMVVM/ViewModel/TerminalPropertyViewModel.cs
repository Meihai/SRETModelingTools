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
    public class TerminalPropertyViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the TerminalViewModel class.
        /// </summary>
        public TerminalPropertyViewModel()
        {
            PropertyModel = new TerminalPropertyModel();
        }

        private TerminalPropertyModel propertyModel;

        public TerminalPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { propertyModel = value; }
        }

    }
}