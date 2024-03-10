using SpaceSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicalWpf
{
    public class SolarsystemCanvas : Canvas
    {
        public Solarsystem solarsystem { get; set; }

        public DrawingObject selectedObject { get; set; }

        public double scale { get; set; }
        public SolarsystemCanvas(double WindowWidth, double WindowHeight, double scale)
        {
            this.scale = scale;
            this.solarsystem = new();
            this.Background = new SolidColorBrush(Colors.Black);
            this.selectedObject = createDrawingObject(solarsystem.star);
        }

        private DrawingObject createDrawingObject(SpaceObject spaceObject)
        {
            Ellipse ellipse = new Ellipse();
            this.Children.Add(ellipse);
            DrawingObject newDrawingObject = new DrawingObject(spaceObject, ellipse);
            foreach (var child in spaceObject.children)
            {
                newDrawingObject.children.Add(createDrawingObject(child));
            }
            return newDrawingObject;
        }

        public void Draw(double newSideLength)
        {
            this.Width = this.Height = newSideLength;
            this.selectedObject.Draw(this);
        }
    }
    public class DrawingObject
    {
        public SpaceObject spaceObject { get; set; }
        public Ellipse ellipse { get; set; }
        public List<DrawingObject> children { get; set; }

        public DrawingObject(SpaceObject spaceObject, Ellipse ellipse)
        {
            this.spaceObject = spaceObject;
            this.ellipse = ellipse;
            Color color = (Color)ColorConverter.ConvertFromString(spaceObject.color);
            ellipse.Fill = new SolidColorBrush(color);
            this.children = new List<DrawingObject>();
        }
        public void Draw(SolarsystemCanvas solarsystemCanvas)
        {

            double factor = solarsystemCanvas.solarsystem.calculateRadius() * 2 / solarsystemCanvas.Width * 1.05;
            ellipse.Height = ellipse.Width = solarsystemCanvas.scale * spaceObject.radius / factor;
            Position canvasPosition = SolarsystemPosToCanvasPos(spaceObject, solarsystemCanvas);
            Canvas.SetTop(ellipse, canvasPosition.Y);
            Canvas.SetLeft(ellipse, canvasPosition.X);
            foreach (DrawingObject child in children) { child.Draw(solarsystemCanvas); }
        }
        public Position SolarsystemPosToCanvasPos(SpaceObject spaceObject, SolarsystemCanvas solarsystemCanvas)
        {
            double factor = solarsystemCanvas.solarsystem.calculateRadius() * 2 / solarsystemCanvas.Width * 1.05;
            Position CanvasPos = new Position();
            CanvasPos.X = -ellipse.Width / 2 + spaceObject.position.X / factor + solarsystemCanvas.Width / 2;
            CanvasPos.Y = -ellipse.Height / 2 + spaceObject.position.Y / factor + solarsystemCanvas.Height / 2;
            return CanvasPos;
        }
    }
}
