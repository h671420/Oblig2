using System.Reflection;
using System.Runtime.CompilerServices;
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
        public SolarsystemCanvas solarSystemCanvas { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
            solarSystemCanvas = new(this.Width, this.Height, scaleSlider.Value);
            mainGrid.Children.Add(solarSystemCanvas);
            Grid.SetColumn(solarSystemCanvas, 1);
            Grid.SetRow(solarSystemCanvas, 0);

            Config config = new(this);

            PlanetBrowser.Content = createStackPanel(solarSystemCanvas.selectedObject);
        }
        private StackPanel createStackPanel(DrawingObject drawingObject)
        {
            StackPanel panel = new();
            Button button = new()
            {
                Content = drawingObject.spaceObject.GetType().Name + " " + drawingObject.spaceObject.name + " children: " + drawingObject.children.Count,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
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
                            if (element is not Button)
                                element.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case Key.Right:
                        foreach (UIElement element in panel.Children)
                        {
                            if (element is not Button)
                                element.Visibility = Visibility.Visible;
                        }
                        break;
                }
            };
            button.Click += (sender, args) =>
            {
                removeEllipses(solarSystemCanvas.selectedObject);
                addEllipses(drawingObject);
                solarSystemCanvas.selectedObject = drawingObject;
            };
            panel.Children.Add(button);
            foreach (var child in drawingObject.children)
            {
                panel.Children.Add(createStackPanel(child));
            }
            return panel;
        }
        public void removeEllipses(DrawingObject drawingObject)
        {
            solarSystemCanvas.Children.Remove(drawingObject.ellipse);
            foreach (var child in drawingObject.children)
                removeEllipses(child);
        }
        public void addEllipses(DrawingObject drawingObject)
        {
            solarSystemCanvas.Children.Add(drawingObject.ellipse);
            foreach (var child in drawingObject.children)
                addEllipses(child);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.solarSystemCanvas.Draw(Math.Min(e.NewSize.Width, e.NewSize.Height - 36));
        }
    }
    public class Config
    {
        private Window window;
        private DispatcherTimer timer;


        private double FramesPerSecond;
        public void setFramesPerSecond(double fps)
        {
            if (fps < 10)
            {
                timer.Stop();
            }
            else if (fps > 120)
            {
                //do nothing
            }
            else
            {
                FramesPerSecond = fps;
            }
        }
        private double DaysPerSecond;
        private double DistanceScale;

        public Config(MainWindow window)
        {
            this.window = window;
            this.timer = new DispatcherTimer();
            this.DistanceScale = 60;
            this.DaysPerSecond = 0;
            this.FramesPerSecond = 60;
            timer.Interval = timeSpanFromFps();
            timer.Tick += (sender, args) => {
                window.solarSystemCanvas.solarsystem.setDay(window.solarSystemCanvas.solarsystem.getDay() + DaysPerSecond / FramesPerSecond);
                window.solarSystemCanvas.Draw(window.solarSystemCanvas.Width);
            };
            timer.Start();
            window.scaleSlider.ValueChanged += (sender, args) =>
            {
                window.solarSystemCanvas.setScale(args.NewValue);
            };
            window.timeScale.ValueChanged += (sender, args) =>
            {
                DaysPerSecond = args.NewValue;
            };
            window.frameScale.ValueChanged += (sender, args) =>
            {
                setFramesPerSecond(args.NewValue);
            };
        }
        public TimeSpan timeSpanFromFps()
        {
            return TimeSpan.FromSeconds(1 / FramesPerSecond);
        }
    }
}