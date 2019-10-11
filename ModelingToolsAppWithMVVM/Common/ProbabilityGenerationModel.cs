using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 概率生成模型
    /// </summary>
    public class ProbabilityGenerationModel
    {
        private List<ProbabilityEvent> events;

        public List<ProbabilityEvent> Events
        {
            get { return events; }
            set { events = value; }
        }


        /// <summary>
        /// 检查概率和的合法性
        /// </summary>
        /// <returns></returns>
        private bool CheckSumValid() {
            double sum = 0;
            bool isValid = false;
            double difference = 1e-6;
            for (int i = 0; i < events.Count; i++) {
                sum = sum + events[i].Probability;
            }
            if (Math.Abs(sum - 1) < difference) {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// 检查单个数值的合法性
        /// </summary>
        /// <returns></returns>
        private bool CheckSingleValid()
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].Probability < 0 || events[i].Probability > 1.0)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 生成概率事件
        /// </summary>
        /// <returns>返回一个具体的概率事件</returns>
        public ProbabilityEvent generateEvent()
        {
            if(CheckSumValid() && CheckSingleValid()){
                ProbabilityEvent insProbEvent = generateRandomEventByProbability();
                return insProbEvent;
            }
            return null;
        }

        //实现一个均匀分布函数 
        private double AverageRandom(double min, double max) {
            int minInteger = (int)(min * 10000);
            int maxInteger = (int)(max * 10000);
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            Random random = new Random(iSeed);
            int randInteger = random.Next() * random.Next();
            int diffInteger = maxInteger - minInteger;
            int resultInteger = randInteger % diffInteger + minInteger;
            return (resultInteger / 10000.0);
        }

        /// <summary>
        /// 根据指定的概率分布生成一个概率事件
        /// </summary>
        /// <returns></returns>
        private ProbabilityEvent generateRandomEventByProbability() { 
            List<double> probList=new List<double>();
            //确定每个概率区间           
            double probSum=0;
            probList.Add(probSum);
            for (int i = 0; i < events.Count; i++)
            {
                probSum = probSum + events[i].Probability;
                probList.Add(probSum);//probList所有数据会是同一个值么？待测试
            }
            double actualValue = AverageRandom(0, 1.0);
            for (int i = 1; i < probList.Count; i++)
            {
                if (actualValue <= probList[i]) {
                    return events[i - 1];
                }
            }
            //未找到对应的概率事件返回null
            return null;
        }

    }
}
