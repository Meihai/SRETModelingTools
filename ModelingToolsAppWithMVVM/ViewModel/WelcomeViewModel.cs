using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    public class WelcomeViewModel:ViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WelcomeViewModel() {
            Welcome = new WelcomeModel() { Introduction="Hello MVVMLight!"};
        }

        #region 属性
        private WelcomeModel welcome;
        /// <summary>
        /// 欢迎词属性
        /// </summary>
        public WelcomeModel Welcome
        {
            get { return welcome; }
            set { welcome = value;
                  RaisePropertyChanged(() => Welcome);
            }
        }
        #endregion
    }
}
