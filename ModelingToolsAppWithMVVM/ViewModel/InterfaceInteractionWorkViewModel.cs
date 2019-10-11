using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Common.ChartCommon;
using ModelingToolsAppWithMVVM.Common.ServiceModel;
using ModelingToolsAppWithMVVM.Model;
using ModelingToolsAppWithMVVM.View;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InterfaceInteractionWorkViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the InterfaceInteractionWorkViewModel class.
        /// </summary>
        public InterfaceInteractionWorkViewModel()
        {
            init();
            dealWithEvent();
        }

        private void init() {
            messageView = new MessageManagerUserControlView();
            propertyView = new InterfaceCrossLinkPropertyView();
            workPanelView = new InterfaceInteractionWorkModel();
            ((Canvas)workPanelView).AllowDrop = true;
            ((Canvas)workPanelView).Focusable=true;
            ((Canvas)workPanelView).Background=new SolidColorBrush(Colors.White);
            ((Canvas)workPanelView).MinHeight=1320;
            ((Canvas)workPanelView).MinWidth=1000;   
     
                             
         
        }
        private object propertyView;

        public object PropertyView
        {
            get { return propertyView; }
            set {
                propertyView = value;
                RaisePropertyChanged(() => PropertyView);
            }
        }
        private object messageView;

        public object MessageView
        {
            get { return messageView; }
            set {
                messageView = value;
                RaisePropertyChanged(() => MessageView);
            }
        }

        private object workPanelView;

        public object WorkPanelView
        {
            get { return workPanelView; }
            set { 
                workPanelView = value;
                RaisePropertyChanged(() => WorkPanelView);
            }
        }

        private void dealWithEvent()
        {
            if (workPanelView is InterfaceInteractionWorkModel)
            {
                ((InterfaceInteractionWorkModel)workPanelView).evtShapeSelected += IIWMSelectedChartChanged;
            }
        }


        /// <summary>
        /// 软件接口模型选择图变化
        /// </summary>
        /// <param name="selectedObject"></param>
        private void IIWMSelectedChartChanged(IFlowChartBase selectedObject) {
            switch (selectedObject.FlowChartType){
                case (FlowChartTypes.InterfaceInteractionLink):
                    {
                        InterfaceInteractionLink ins = (InterfaceInteractionLink)selectedObject;
                        //IILinkSM iiLinkSM = (IILinkSM)ins.PropertyModel;
                        PropertyView = new InterfaceCrossLinkPropertyView();
                        InterfaceCrossLinkPropertyModel iiLinkModel = new InterfaceCrossLinkPropertyModel(ins);
                        InterfaceCrossLinkPropertyViewModel iiLinkViewModel = new InterfaceCrossLinkPropertyViewModel();
                        iiLinkViewModel.CrossLinkProperty = iiLinkModel;
                        ((UserControl)PropertyView).DataContext = iiLinkViewModel;
                        break;
                    }
                case(FlowChartTypes.InterfaceInteractionObject ):{
                    InterfaceInteractionObject ins = (InterfaceInteractionObject)selectedObject;
                    PropertyView = new InterfaceInteractionObjectPropertyView();
                    IIObjectSM iiObjectSM = (IIObjectSM)ins.PropertyModel;
                    InterfaceInteractionObjectModel iiObjectModel = new InterfaceInteractionObjectModel(iiObjectSM);
                    InterfaceInteractionObjectViewModel iiObjectViewModel = new InterfaceInteractionObjectViewModel();
                    iiObjectViewModel.InterfaceInteractionModel = iiObjectModel;
                    ((UserControl)PropertyView).DataContext = iiObjectViewModel;
                    break;
                }
                case(FlowChartTypes.InterfaceTestedObject):{
                    InterfaceInteractionTestedObject ins = (InterfaceInteractionTestedObject)selectedObject;
                    IITestedObjectSM iiTestedObjectSM = (IITestedObjectSM)ins.PropertyModel;
                    InterfaceInteractionTestedObjectPropertyModel iiTestedModel = new InterfaceInteractionTestedObjectPropertyModel(iiTestedObjectSM);
                    InterfaceInteractionTestedObjectPropertyViewModel iiTestedViewModel = new InterfaceInteractionTestedObjectPropertyViewModel();
                    iiTestedViewModel.PropertyModel = iiTestedModel;
                    PropertyView = new InterfaceInteractionTestedObjectPropertyView();
                    ((UserControl)PropertyView).DataContext = iiTestedViewModel;
                    break;
                }
                default:{
                    break;
                }
   
            }
            
              
            
        }
        

        
    }
}