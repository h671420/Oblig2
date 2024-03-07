using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SpaceSim;

namespace GraphicalWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolarsystemCanvas canvas;

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;

            canvas= new(this.Width, this.Height);
            this.Content = canvas;
            //defaultCanvas.Children.Add();


        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.canvas.Width = this.canvas.Height = Math.Min(e.NewSize.Width,e.NewSize.Height)*.95;
            
        }
    }
    public class SolarsystemCanvas : Canvas
    {
        public SolarsystemCanvas(double WindowWidth, double WindowHeight) 
        {
            //this.Width = this.Height = 400;//Math.Min(WindowWidth, WindowHeight) * .95;
            this.Background = new SolidColorBrush(Colors.Black);
        }

    }
}