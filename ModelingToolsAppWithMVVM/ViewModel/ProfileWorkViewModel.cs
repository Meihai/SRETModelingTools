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
    public class ProfileWorkViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ProfileWorkViewModel class.
        /// </summary>
        public ProfileWorkViewModel()
        {
            init();
        }

        private void init()
        {          
            propertyView = new InterfaceInteractionObjectPropertyView();
            workPanelView = new ProfileWorkModel();
            ((Canvas)workPanelView).AllowDrop = true;
            ((Canvas)workPanelView).Focusable = true;
            ((Canvas)workPanelView).Background = new SolidColorBrush(Colors.White);
            ((Canvas)workPanelView).MinHeight = 1320;
            ((Canvas)workPanelView).MinWidth = 1000;
            dealWithEvent();            
        }
        private object propertyView;

        public object PropertyView
        {
            get { return propertyView; }
            set
            {
                propertyView = value;
                RaisePropertyChanged(() => PropertyView);
            }
        }
        private object messageView;

        public object MessageView
        {
            get { return messageView; }
            set
            {
                messageView = value;
                RaisePropertyChanged(() => MessageView);
            }
        }

        private object workPanelView;

        public object WorkPanelView
        {
            get { return workPanelView; }
            set
            {
                workPanelView = value;
                RaisePropertyChanged(() => WorkPanelView);
            }
        }

        /// <summary>
        /// 事件处理函数
        /// </summary>
        private void dealWithEvent()
        {
            if (WorkPanelView is ProfileWorkModel)
            {
                ((ProfileWorkModel)workPanelView).evtShapeSelected += IIWMSelectedChartChanged;
            }
        }

         /// <summary>
        /// 软件接口模型选择图变化
        /// </summary>
        /// <param name="selectedObject"></param>
        private void IIWMSelectedChartChanged(IFlowChartBase selectedObject)
        {
            switch (selectedObject.FlowChartType)
            {
                case (FlowChartTypes.ShapeSeqTransfer):
                    {
                        LinkSeqTransfer ins = (LinkSeqTransfer)selectedObject;
                        PropertyView = new SeqTransferPropertyView();
                        SeqTransferPropertyModel propertyModel = new SeqTransferPropertyModel(ins);
                        SeqTransferPropertyViewModel propertyViewModel = new SeqTransferPropertyViewModel();
                        propertyViewModel.PropertyModel = propertyModel;
                        ((UserControl)PropertyView).DataContext = propertyViewModel;
                        break;
                    }
                case (FlowChartTypes.ShapeProbTransfer):
                    {
                        LinkProbTransfer ins = (LinkProbTransfer)selectedObject;
                        PropertyView = new ProbTransferPropertyView();                      
                        ProbTransferPropertyModel propertyModel = new ProbTransferPropertyModel(ins);
                        ProbTransferPropertyViewModel propertyViewModel = new ProbTransferPropertyViewModel();
                        propertyViewModel.PropertyModel = propertyModel;
                        ((UserControl)PropertyView).DataContext = propertyViewModel;
                        break;
                    }
                case (FlowChartTypes.ShapeEntrance):
                    {
                        ShapeEntrance ins = (ShapeEntrance)selectedObject;
                        PropertyView = new EntrancePropertyView();
                        EntrancePropertyModel propertyModel = new EntrancePropertyModel(ins);
                        EntrancePropertyViewModel propertyViewModel = new EntrancePropertyViewModel();
                        propertyViewModel.PropertyModel = propertyModel;                      
                        ((UserControl)PropertyView).DataContext = propertyViewModel;
                        break;
                    }
                case (FlowChartTypes.ShapeExit):
                {
                    ShapeExit ins=(ShapeExit)selectedObject;
                    PropertyView=new ExitPropertyView();
                    ExitPropertyModel propertyModel=new ExitPropertyModel(ins);
                    ExitPropertyViewModel propertyViewModel=new ExitPropertyViewModel();
                    propertyViewModel.PropertyModel=propertyModel;
                    ((UserControl)PropertyView).DataContext=propertyViewModel;
                    break;
                }
                
                case (FlowChartTypes.ShapeTerminal):
                {
                    ShapeTerminal ins = (ShapeTerminal)selectedObject;
                    PropertyView = new TerminalPropertyView();
                    TerminalPropertyModel propertyModel = new TerminalPropertyModel(ins);
                    TerminalPropertyViewModel propertyViewModel = new TerminalPropertyViewModel();
                    propertyViewModel.PropertyModel = propertyModel;
                    ((UserControl)PropertyView).DataContext = propertyViewModel;
                    break;
                }

                case (FlowChartTypes.ShapeOperation):
                {
                    ShapeOperation ins = (ShapeOperation)selectedObject;
                    PropertyView = new OperationPropertyView();
                    OperationPropertyModel propertyModel = new OperationPropertyModel(ins);
                    OperationPropertyViewModel propertyViewModel = new OperationPropertyViewModel();
                    propertyViewModel.PropertyModel = propertyModel;
                    ((UserControl)PropertyView).DataContext = propertyViewModel;
                    break;
                }

                case (FlowChartTypes.ShapeReliableProfile):
                {
                    ShapeReliableProfile ins = (ShapeReliableProfile)selectedObject;
                    PropertyView = new ReliableProfilePropertyView();
                    ReliableProfilePropertyModel propertyModel = new ReliableProfilePropertyModel(ins);
                    ReliableProfilePropertyViewModel propertyViewModel = new ReliableProfilePropertyViewModel();
                    propertyViewModel.PropertyModel = propertyModel;
                    ((UserControl)PropertyView).DataContext = propertyViewModel;
                    break;
                }

                default:
                    {
                        break;
                    }
            }
        }
            

    }
}