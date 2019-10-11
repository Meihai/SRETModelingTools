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


namespace ModelingToolsApp
{
    /// <summary>
    /// Interaction logic for ColorSelectionWindow.xaml
    /// </summary>
    public partial class ColorSelectionWindow : Window
    {
        public ColorSelectionWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

          private void Rectangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Brush brush = (sender as Rectangle).Fill;
            Color c = (Color)ColorConverter.ConvertFromString(brush.ToString());
            ExSelectedBrush = brush;
            ExSelectedColor = c;
           
        }

        public event ColorChangedHandler ColorChangedEvent;

        public event BrushChangedHandler BrushChangedEvent;

      
        public static readonly DependencyProperty ExSelectedColorProperty = DependencyProperty.Register(
                     "ExSelectedColor",
                     typeof(Color),
                     typeof(ColorSelectionWindow),
                     new PropertyMetadata(Colors.Black));

        public Color ExSelectedColor
        {
            get { return (Color)GetValue(ExSelectedColorProperty); }
            set { SetValue(ExSelectedColorProperty, value); }
        }

        public static readonly DependencyProperty ExSelectedBrushProperty = DependencyProperty.Register(
                      "ExSelectedBrush",
                      typeof(Brush),
                      typeof(ColorSelectionWindow),
                      new PropertyMetadata(Brushes.Black));

        public Brush ExSelectedBrush
        {
            get { return (Brush)GetValue(ExSelectedBrushProperty); }
            set { SetValue(ExSelectedBrushProperty, value); }
        }

        private void Click_Save(object sender, RoutedEventArgs e) {
            if(null !=ColorChangedEvent){
                
                ColorChangedEvent(sender, ExSelectedColor);
            }
            if(null !=BrushChangedEvent){
                BrushChangedEvent(sender, ExSelectedBrush);
            }
            this.Close();
           
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    public delegate void ColorChangedHandler(object sender, Color newColor);

    public delegate void BrushChangedHandler(object sender, Brush newBrush);
    
}
