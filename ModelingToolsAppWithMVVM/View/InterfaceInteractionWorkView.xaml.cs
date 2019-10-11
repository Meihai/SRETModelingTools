using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Interaction logic for InterfaceInteractionWorkView.xaml
    /// </summary>
    public partial class InterfaceInteractionWorkView : UserControl
    {
        public InterfaceInteractionWorkView()
        {
            InitializeComponent();
        }

        private void btnShowPropertyClick(object sender,RoutedEventArgs e){
            if(brdProperty.Width==0){
                while(brdProperty.Width<200){
                    brdProperty.Width+=1;
                }
                brdProperty.Width=200;
            }
            else if(brdProperty.Width>0){
                while(brdProperty.Width>1){
                    brdProperty.Width-=1;
                }
                brdProperty.Width=0;
            }
        }


        private void btnMessageManagerShowPropertyClick(object sender, RoutedEventArgs e)
        {
           // this.messageManagerBox.Content = new MessageManagerUserControlView();
            if (brdMessageManagerProperty.Height == 0)
            {
                while (brdMessageManagerProperty.Height < 200)
                {
                    brdMessageManagerProperty.Height += 1;
                }
                brdMessageManagerProperty.Height = 200;
            }
            else if (brdMessageManagerProperty.Height > 0)
            {
                while (brdMessageManagerProperty.Height > 1)
                {
                    brdMessageManagerProperty.Height -= 1;
                }
                brdMessageManagerProperty.Height = 0;
            }
        }
        

    }
}
