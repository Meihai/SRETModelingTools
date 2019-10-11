using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{   
    /// <summary>
    /// 与时间相关的连续变量
    /// </summary>
    public class ContinuousVariable
    {
        private double minValue;   

        private double maxValue;
      
        private double sampleFrequency;
      
        private FunctionExpressionParser valueExpression;

        private double happenTime;

        public double HappenTime
        {
            get { return happenTime; }
            set { happenTime = value; }
        }


        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public double SampleFrequency
        {
            get { return sampleFrequency; }
            set { sampleFrequency = value; }
        }


        public FunctionExpressionParser ValueExpression
        {
            get { return valueExpression; }
            set { valueExpression = value; }
        }

        /// <summary>
        /// 当前值处理方式,委托事件
        /// </summary>
        /// <param name="currentValue"></param>
        public delegate void DealWithCurrentValueEvent(double currentValue);

        /// <summary>
        /// 观察者需要实现该事件以接收值随时间变化的情况
        /// </summary>
        public event DealWithCurrentValueEvent CurrentValueDealEvent;

        public void Notify() {
            if (CurrentValueDealEvent != null) { 
                //TODO 根据函数表达式和定时器生成连续值
                double currentValue=0.0;
                CurrentValueDealEvent(currentValue);

            }
        }

        #region 定时器服务
        /// <summary>
        /// 启动定时器服务
        /// </summary>
        /// <param name="autoFlag">
        /// autoFlag 为false,执行一次 autoFlag为true,一直执行
        /// </param>
        public void StartTimer(bool autoFlag) {
            happenTime = 0;
            double timeInterval = 1.0 / sampleFrequency*1000.0;//转化为毫秒的时间单位
            System.Timers.Timer t = new System.Timers.Timer(timeInterval);
            t.Elapsed += new System.Timers.ElapsedEventHandler(CalcCurrentValue);
            t.AutoReset = autoFlag;//设置是执行一次(false)还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        //到达时间的时候的执行事件
        public void CalcCurrentValue(object source, System.Timers.ElapsedEventArgs e)
        {
            double timeInterval = 1.0 / sampleFrequency;//转化为秒的时间单位
            happenTime = happenTime + timeInterval;
            Notify();
        }

        #endregion







    }
}
