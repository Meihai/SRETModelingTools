using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    public class RandomStringBuilder
    {
        /// <summary>
        /// 生成单个随机数字
        /// </summary>
        /// <returns></returns>
        public static int CreateNum() {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int num = random.Next(10);
            return num;
        }

        /// <summary>
        /// 生成单个大写随机字符
        /// </summary>
        /// <returns></returns>
        public static string CreateBigAbc()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int num = random.Next(65, 91);
            string abc = Convert.ToChar(num).ToString();
            return abc;
        }

        /// <summary>
        /// 生成单个小写随机字母
        /// </summary>
        /// <returns></returns>
        public static string CreateSmallAbc()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int num = random.Next(97, 123);
            string abc = Convert.ToChar(num).ToString();
            return abc;
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Create(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                switch (random.Next(3))
                {
                    case 0:
                        {
                            sb.Append(CreateNum());
                            break;
                        }
                    case 1:
                        {
                            sb.Append(CreateSmallAbc());
                            break;
                        }
                    case 2:
                        {
                            sb.Append(CreateBigAbc());
                            break;
                        }
                    default:
                        break;
                }
            }
            return sb.ToString();
        }

    }
}
