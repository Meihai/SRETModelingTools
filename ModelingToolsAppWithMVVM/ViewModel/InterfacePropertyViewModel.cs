using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InterfacePropertyViewModel : ViewModelBase
    {
      
        /// <summary>
        /// Initializes a new instance of the InterfacePropertyViewModel class.
        /// </summary>
        public InterfacePropertyViewModel()
        {
            init();
           
        }

        
        private void init() {
            _interfaceConfig = new InterfaceConfig();
            _interfaceConfigList = new List<InterfaceConfig>();
            _interfaceConfigList.Add(_interfaceConfig);

        }

        private string _interfacePropertyTitle="软件交联接口属性窗口";

        public string InterfacePropertyViewTitle {
            get { return _interfacePropertyTitle; }
            set { _interfacePropertyTitle = value;
                  RaisePropertyChanged(() => InterfacePropertyViewTitle);
                }
        }


        private List<InterfaceConfig> _interfaceConfigList;

        public List<InterfaceConfig> InterfaceConfigList
        {
            get { return _interfaceConfigList; }
            set { _interfaceConfigList = value;
                   RaisePropertyChanged(() => InterfaceConfigList);
            }
        }

        private InterfaceConfig selectedItem;

        public InterfaceConfig SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        private InterfaceConfig _interfaceConfig;

        public InterfaceConfig InterfaceInfo
        {
            get { return _interfaceConfig; }
            set { 
                  _interfaceConfig = value;                  
                  RaisePropertyChanged(() => InterfaceInfo);
                }
        }

         
    }
}