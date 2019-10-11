using System.Windows;
using System.Windows.Controls;

namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Description for ProfileWorkView.
    /// </summary>
    public partial class ProfileWorkView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ProfileWorkView class.
        /// </summary>
        public ProfileWorkView()
        {
            InitializeComponent();
        }

        private void btnShowPropertyClick(object sender, RoutedEventArgs e)
        {
            if (brdProperty.Width == 0)
            {
                while (brdProperty.Width < 200)
                {
                    brdProperty.Width += 1;
                }
                brdProperty.Width = 200;
            }
            else if (brdProperty.Width > 0)
            {
                while (brdProperty.Width > 1)
                {
                    brdProperty.Width -= 1;
                }
                brdProperty.Width = 0;
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