using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Manager
{
    /// <summary>
    /// 工厂单例实现类
    /// </summary>
    public sealed class FactoryImpl:IFactory
    {
        private static readonly FactoryImpl instance = null;
        static FactoryImpl(){
            instance=new FactoryImpl();
        }
        private FactoryImpl()
        {

        }

        public static FactoryImpl Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
