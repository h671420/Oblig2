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

            this.SizeChanged += MainWindow_SizeChanged;

            canvas= new(this.Width, this.Height);
            this.Content = canvas;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            canvas.solarsystem.setDay(canvas.solarsystem.getDay()+10);
            canvas.Draw(canvas.Width);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.canvas.Draw(Math.Min(e.NewSize.Width, e.NewSize.Height - 35));   
        }
    }
    public class SolarsystemCanvas : Canvas
    {
        public Solarsystem solarsystem { get; set; }

        List<DrawingObject> drawingObjects = new List<DrawingObject>();
        public SolarsystemCanvas(double WindowWidth, double WindowHeight) 
        {
            this.solarsystem = new();
            this.Background = new SolidColorBrush(Colors.Black);

            foreach (var item in solarsystem.spaceObjects)
            {
                Ellipse ellipse = new();
                this.Children.Add(ellipse); 
                DrawingObject drawing = new(item,ellipse);
                drawingObjects.Add(drawing);

            }
        }
        public void Draw(double newSideLength)
        {
            this.Width = this.Height= newSideLength;
            foreach (var drawingObject in drawingObjects)
            {
                drawingObject.Draw(this);
            }
        }
    }
    public class DrawingObject
    {
        //infobox
        public SpaceObject spaceObject { get; set; }
        public Ellipse ellipse { get; set; }

        public DrawingObject(SpaceObject spaceObject, Ellipse ellipse)
        {   
            this.spaceObject = spaceObject;
            this.ellipse = ellipse;
            Color color = (Color)ColorConverter.ConvertFromString(spaceObject.color);
            ellipse.Fill = new SolidColorBrush(color); 
        }
        public void Draw(SolarsystemCanvas solarsystemCanvas) {
            
            double factor = solarsystemCanvas.solarsystem.calculateRadius() * 2 / solarsystemCanvas.Width * 1.05;
            ellipse.Height = ellipse.Width = spaceObject.radius/factor;
            ellipse.Height = ellipse.Width = 5;
            Position canvasPosition = SolarsystemPosToCanvasPos(spaceObject, solarsystemCanvas);
            Canvas.SetTop(ellipse, canvasPosition.Y);
            Canvas.SetLeft(ellipse, canvasPosition.X);
        }
        public Position SolarsystemPosToCanvasPos(SpaceObject spaceObject,SolarsystemCanvas solarsystemCanvas)
        {
            double factor = solarsystemCanvas.solarsystem.calculateRadius() * 2 / solarsystemCanvas.Width * 1.05;
            Position CanvasPos = new Position();
            CanvasPos.X = -spaceObject.radius / 2*factor + spaceObject.position.X / factor + solarsystemCanvas.Width / 2;
            CanvasPos.Y = -spaceObject.radius / 2*factor + spaceObject.position.Y / factor + solarsystemCanvas.Height / 2;
            CanvasPos.X = -2.5 + spaceObject.position.X / factor + solarsystemCanvas.Width / 2;
            CanvasPos.Y = -2.5 + spaceObject.position.Y / factor + solarsystemCanvas.Height / 2;
            return CanvasPos;
        }
    }
}