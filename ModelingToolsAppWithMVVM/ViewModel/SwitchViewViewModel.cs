using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ModelingToolsAppWithMVVM.View;
using System.Windows.Input;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SwitchViewViewModel : ViewModelBase
    {
        private ICommand _gotoView1Command;
        private ICommand _gotoView2Command;
        private object _currentView;
        private object _view1;
        private object _view2; 
        
        /// <summary>
        /// Initializes a new instance of the SwitchViewViewModel class.
        /// </summary>
        public SwitchViewViewModel()
        {
            _view1 = new SwitchView1();
            _view2 = new SwitchView2();

            CurrentView = _view2;
        }

        public object GotoView1Command
        {
            get
            {
                return _gotoView1Command ?? (_gotoView1Command = new RelayCommand(

                        GotoView1, () => { return true; }
                   
                    ));
            }
        }

        public ICommand GotoView2Command
        {
            get
            {
                return _gotoView2Command ?? (_gotoView2Command = new RelayCommand(

                       GotoView2, () => { return true; }
                   ));
            }
        }


        public object CurrentView
        {
             get { return _currentView; }
             set
             {
                 _currentView = value;
                 RaisePropertyChanged(() => CurrentView);
                 //OnPropertyChanged("CurrentView");
             }
        }
 
        private void GotoView1()
        {
            CurrentView = _view1;
        }
 
        private void GotoView2()
        {
             CurrentView =  _view2;
        }
   

    }
}