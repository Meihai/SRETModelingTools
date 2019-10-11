using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public class FlowChartUtil
    {
        public static IFlowChartBase GetFlowChartBase(string modelName)
        {
            IFlowChartBase ifcb = null;
            switch (modelName)
            {
                case "ShapeSeqTransfer":{
                    ifcb = new LinkSeqTransfer(LineTypes.Solid);
                    break;
                }                  
                case "ShapeProbTransfer":{
                    ifcb = new LinkProbTransfer(LineTypes.ShortDashes);
                    break;
                }                   
                case "ShapeEntrance": {
                    ifcb=new ShapeEntrance();
                    break;
                }
                case "ShapeExit": {
                    ifcb = new ShapeExit();
                    break;
                }
                case "ShapeTerminal":{
                    ifcb = new ShapeTerminal();
                    break;
                }
                case "ShapeOperation":{
                    ifcb = new ShapeOperation();
                    break;
                }             
                case "ShapeReliableProfile":{
                   ifcb = new ShapeReliableProfile();
                   break;
                }               
                case "ShapeConditionBranch":{
                    // todo
                    break;
                }
                case "InterfaceInteractionLink": {
                    ifcb = new InterfaceInteractionLink();
                    break;
                }
                case "InterfaceInteractionObject": {
                    ifcb = new InterfaceInteractionObject();
                    break;
                }
                case "InterfaceTestedObject":{
                    ifcb=new InterfaceInteractionTestedObject();
                    break;
                }
                default:
                    break;
            }
            return ifcb;
        }

             
              
    }
}
