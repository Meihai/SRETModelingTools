using ModelingToolsAppWithMVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    public class InterfaceInteractionTestedObjectPropertyViewModel:ViewModelBase
    {
        public InterfaceInteractionTestedObjectPropertyViewModel()
        {
            init();
        }

        private void init(){
            propertyModel=new InterfaceInteractionTestedObjectPropertyModel();
        }

        private InterfaceInteractionTestedObjectPropertyModel propertyModel;

        public InterfaceInteractionTestedObjectPropertyModel PropertyModel
        {
            get { return propertyModel; }
            set { 
                propertyModel = value;
                RaisePropertyChanged(() => PropertyModel);
            }
        }

    }
}
