using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 时间离散变量,只考虑数值型时间离散变量
    /// </summary>
    public abstract class DiscreteVariable
    {
        private List<long> timeList;

        public List<long> TimeList
        {
            get { return timeList; }
            set { timeList = value; }
        }



        private List<double> dataList;

        public List<double> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }

        /// <summary>
        /// 根据时间获取离散变量的值
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public abstract double getValue(long time);
       
        
    }
}
