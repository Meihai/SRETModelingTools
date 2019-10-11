using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 函数表达式解析类
    /// </summary>
    public class FunctionExpressionParser
    {
        private List<string> variableSymbols;      
        private string symbolicExpression;

      
        public  bool isValid(){            
            return true;//默认合法
        }

        

        public double calculate(List<IndependentVariable> vals){
            //todo 根据给定的变量生成对应的值
            return 0;//需要更改的
        }

        public List<string> VariableSymbols
        {
            get { return variableSymbols; }
            set { variableSymbols = value; }
        }

        public string SymbolicExpression
        {
            get { return symbolicExpression; }
            set { symbolicExpression = value; }
        }

    }
}
