using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Threading;
using SpaceSim;

namespace GraphicalWpf
{
    public partial class MainWindow : Window
    {
        SolarsystemCanvas canvas;
        public MainWindow()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.ThreeDBorderWindow;
            this.SizeChanged += MainWindow_SizeChanged;


            scaleSlider.ValueChanged += ScaleSlider_ValueChanged;
            canvas = new(this.Width, this.Height, scaleSlider.Value);
            mainGrid.Children.Add(canvas);
            Grid.SetColumn(canvas, 1);
            Grid.SetRow(canvas, 0);

            scrollViewer.VerticalScrollBarVisibility= ScrollBarVisibility.Auto;
            scrollViewer.Content = createStackPanel(canvas.selectedObject);

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private StackPanel createStackPanel(DrawingObject drawingObject)
        {
            StackPanel panel = new StackPanel();
            Button button = new Button();
            button.Content = drawingObject.spaceObject.GetType().Name+ " " + drawingObject.spaceObject.name+ " children: "+drawingObject.children.Count;
            button.HorizontalContentAlignment = HorizontalAlignment.Left;
            button.KeyDown += (sender, args) =>
            {
                Button clickedButton = (Button)sender;
                switch (args.Key)
                {
                    case Key.Enter:
                        break;
                    case Key.Left:
                        foreach (UIElement element in panel.Children)
                        {
                            if (!(element is Button))
                                element.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case Key.Right:
                        foreach (UIElement element in panel.Children)
                        {
                            if (!(element is Button))
                                element.Visibility = Visibility.Visible;
                        }
                        break;
                }
            };
            button.Click += (sender, args) =>
            {
                canvas.selectedObject = drawingObject;
            };
            panel.Children.Add(button);
            foreach (var child in drawingObject.children)
            {
                panel.Children.Add(createStackPanel(child));
            }
            return panel;
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            canvas.scale = slider.Value;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            canvas.solarsystem.setDay(canvas.solarsystem.getDay()+5);
            canvas.Draw(canvas.Width);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.canvas.Draw(Math.Min(e.NewSize.Width, e.NewSize.Height - 36));   
        }
    }
    
    
}